using Barcodes.Core.Abstraction;
using Barcodes.Core.Common;
using Barcodes.Core.Extensions;
using Barcodes.Core.Models;
using Barcodes.Extensions;
using Barcodes.Services.DocExport;
using Barcodes.Services.Storage;
using Prism.Commands;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Barcodes.Core.ViewModels
{
    public class QuickGeneratorViewModel : BaseViewModel, IOnClosingAware
    {
        private readonly IServicesAggregator services;
        private readonly AppViewModel appViewModel;

        private GenerationDataViewModel generationData;
        private BarcodeViewModel barcode;
        private string statusMessage;
        private StorageBarcodeViewModel selectedQuickBarcode;
        private ObservableCollection<StorageBarcodeViewModel> quickBarcodes;
        private StorageBarcodeViewModel emptyQuickBarcode = new StorageBarcodeViewModel(null);
        private string barcodeHeader;

        private DelegateCommand restoreQuickBarcodesCommand;
        private DelegateCommand loadQuickBarcodesCommand;
        private DelegateCommand clearQuickBarcodesCommand;
        private DelegateCommand exportCommand;
        private DelegateCommand printCommand;
        private DelegateCommand addBarcodeToAppCommand;
        private DelegateCommand removeSelectedQuickBarcodeCommand;

        public QuickGeneratorViewModel(IServicesAggregator services, AppViewModel appViewModel)
        {
            this.services = services;
            this.appViewModel = appViewModel;

            services.AppEvents.QuickBarcodeUpdate += AppEvents_QuickBarcodeUpdate;

            generationData = new GenerationDataViewModel(services.AppDialogsService, services.AppWindowsService, services.GeneratorService,
                services.SysService, services.AppSettingsService)
            {
                ParentViewModel = this
            };
            LoadSettings();
            LoadQuickBarcodes();
        }

        public DelegateCommand OpenAppLocationCommand => new DelegateCommand(services.OpenAppLocation);

        public DelegateCommand CloseAppCommand => new DelegateCommand(services.AppWindowsService.CloseShell);

        public DelegateCommand SettingsCommand => new DelegateCommand(() => services.AppWindowsService.ShowSettingsWindow(this));

        public DelegateCommand ExamplesCommand => new DelegateCommand(async () =>
        {
            ExamplesViewModel examplesViewModel = null;
            await HeavyAction("Generating examples...", () =>
            {
                examplesViewModel = services.ContainerExtension.Resolve<ExamplesViewModel>();
                examplesViewModel.ParentViewModel = this;
                return examplesViewModel.CreateExamples();
            });

            services.AppWindowsService.ShowExamplesWindow(this, examplesViewModel);
            if (examplesViewModel.SelectedBarcode != null)
            {
                GenerationData.FromData(examplesViewModel.SelectedBarcode.GenerationData);
                GenerationData.SelectTemplate(examplesViewModel.SelectedBarcode.Template);
            }
        });

        public DelegateCommand AboutCommand => new DelegateCommand(async () => await HeavyAction(null,
            () => services.AppWindowsService.ShowAboutWindow(this, () => BusyMessage = null)));

        public DelegateCommand GenerateCommand => new DelegateCommand(async () => await GenerateBarcode(true));

        public DelegateCommand NewWindowCommand => new DelegateCommand(() => services.AppWindowsService.ShowQuickGeneratorWindow(appViewModel));

        public DelegateCommand CloseAllCommand => new DelegateCommand(services.AppWindowsService.CloseQuickGeneratorsWindows);

        public DelegateCommand SaveImageCommand => new DelegateCommand(() =>
        {
            try
            {
                var filePath = services.AppDialogsService.SavePngFile("barcode");
                if (string.IsNullOrEmpty(filePath))
                {
                    return;
                }

                barcode.Barcode.ToPng(filePath);
                StatusMessage = $"Barcode saved in {Path.GetFileName(filePath)}";
            }
            catch (Exception exc)
            {
                services.AppDialogsService.ShowException("Error when saving barcode to png file", exc);
            }
        });

        public DelegateCommand CopyDataCommand => new DelegateCommand(() =>
        {
            services.SysService.CopyToClipboard(Barcode.GenerationData.Data);
            StatusMessage = "Barcodes data copied to clipboard";
        });

        public DelegateCommand CopyImageCommand => new DelegateCommand(() =>
        {
            services.SysService.CopyToClipboard(Barcode.Barcode);
            StatusMessage = "Barcodes copied to clipboard";
        });

        public DelegateCommand ClearQuickBarcodesCommand => clearQuickBarcodesCommand ?? (clearQuickBarcodesCommand = new DelegateCommand(() =>
        {
            if (services.AppDialogsService.ShowYesNoQuestion("Do you really want to clear previously generated barcodes?"))
            {
                services.StorageService.ClearQuickBarcodes();
                LoadQuickBarcodes();
                NotifyOtherQuickGenerators();
            }
        }, () => QuickBarcodes?.Count > 1));

        public DelegateCommand RestoreQuickBarcodesCommand => restoreQuickBarcodesCommand ?? (restoreQuickBarcodesCommand = new DelegateCommand(async () =>
        {
            GenerationData.FromData(SelectedQuickBarcode.StorageBarcode.ToGenerationData());
            await RestoreBarcode(SelectedQuickBarcode);
        }, () => SelectedQuickBarcode?.StorageBarcode != null));

        public DelegateCommand LoadQuickBarcodesCommand => loadQuickBarcodesCommand ?? (loadQuickBarcodesCommand = new DelegateCommand(async () => await RestoreBarcode(SelectedQuickBarcode),
            () => SelectedQuickBarcode?.StorageBarcode != null));

        public DelegateCommand ResetCommand => new DelegateCommand(() =>
        {
            if (services.AppDialogsService.ShowYesNoQuestion("Do you really want to reset all data?"))
            {
                GenerationData.RestoreSettingsCommand.Execute();
                GenerationData.Data = string.Empty;
                SelectedQuickBarcode = QuickBarcodes.First();
                Barcode = null;
                StatusMessage = null;
            }
        });

        public DelegateCommand ImportCommand => new DelegateCommand(async () =>
        {
            try
            {
                var barcodeFile = services.AppDialogsService.ImportBarcodesFile();
                if (!string.IsNullOrEmpty(barcodeFile))
                {
                    var barcodes = services.StorageService.ImportBarcodes(barcodeFile);
                    StorageBarcodeViewModel barcodeToImport = null;
                    if (barcodes.Count == 0)
                    {
                        services.AppDialogsService.ShowError("No barcodes found in selected file");
                        return;
                    }
                    else if (barcodes.Count == 1)
                    {
                        barcodeToImport = new StorageBarcodeViewModel(barcodes[0]);
                    }
                    else
                    {
                        var selectedBarcode = services.AppWindowsService.SelectStorageBarcode(this,
                            barcodes.Select(b => new StorageBarcodeViewModel(b)).ToList());
                        if (selectedBarcode != null)
                        {
                            barcodeToImport = selectedBarcode;
                        }
                    }

                    if (barcodeToImport != null)
                    {
                        GenerationData.FromData(barcodeToImport.StorageBarcode.ToGenerationData());
                        await GenerateBarcode(true);
                    }
                }
            }
            catch (Exception exc)
            {
                services.LogException("Error when importing barcode", exc);
            }
        });

        public DelegateCommand ExportCommand => exportCommand ?? (exportCommand = new DelegateCommand(() =>
        {
            var barcodeFile = services.AppDialogsService.ExportBarcodesFile();
            if (!string.IsNullOrEmpty(barcodeFile))
            {
                try
                {
                    services.StorageService.ExportBarcodes(barcodeFile, new List<StorageBarcode> { Barcode.ToStorage() });
                    StatusMessage = "Barcode exported successfully";
                }
                catch (Exception exc)
                {
                    services.AppDialogsService.ShowException("Error when exporting barcode", exc);
                }
            }
        }, () => BarcodeVisible));

        public DelegateCommand PrintCommand => printCommand ?? (printCommand = new DelegateCommand(async () =>
        {
            try
            {
                await HeavyAction("Printing barcode...", async () =>
                {
                    await services.DocExportService.PrintAsync(new List<DocBarcodeData> { barcode.ToDocBarcodeData() })
                        .ConfigureAwait(false);
                    StatusMessage = "Successfully printed";
                });
            }
            catch (Exception exc)
            {
                services.LogException("Error when printing barcodes", exc);
            }
        }, () => BarcodeVisible));

        public DelegateCommand AddBarcodeToAppCommand => addBarcodeToAppCommand ?? (addBarcodeToAppCommand = new DelegateCommand(() =>
        {
            var barcodeTitle = services.AppWindowsService.ShowBarcodeTitleWindow(this, title =>
            {
                if (string.IsNullOrEmpty(title))
                {
                    services.AppDialogsService.ShowError("Title can not be empty");
                    return false;
                }
                return true;
            });

            if (!string.IsNullOrEmpty(barcodeTitle))
            {
                appViewModel.AddNewBarcode(new GenerationResult
                {
                    AddNew = true,
                    Barcode = new BarcodeViewModel(Barcode)
                    {
                        Title = barcodeTitle
                    }
                });
            }
        }, () => BarcodeVisible));

        public DelegateCommand RemoveSelectedQuickBarcodeCommand => removeSelectedQuickBarcodeCommand ?? (removeSelectedQuickBarcodeCommand = new DelegateCommand(() =>
        {
            if (services.AppDialogsService.ShowYesNoQuestion("Do you really want to remove selected barcode?"))
            {
                services.StorageService.RemoveQuickBarcode(SelectedQuickBarcode.StorageBarcode);
                LoadQuickBarcodes();
                NotifyOtherQuickGenerators();
            }
        }, () => SelectedQuickBarcode?.StorageBarcode != null));

        public DelegateCommand OpenStorageLocationCommand => new DelegateCommand(() => services.OpenStorageLocation(services.StorageService.QuickBarcodesPath));

        public DelegateCommand OpenLogsCommand => new DelegateCommand(services.OpenLogs);

        public DelegateCommand ShowMainAppWindowCommand => new DelegateCommand(services.AppWindowsService.ShowMainAppWindow);

        public string BarcodeHeader
        {
            get
            {
                var value = "Barcode";
                if (!string.IsNullOrEmpty(barcodeHeader))
                {
                    value += ": " + barcodeHeader;
                }
                return value;
            }
            set => SetProperty(ref barcodeHeader, value);
        }

        public BarcodeViewModel Barcode
        {
            get => barcode;
            set
            {
                SetProperty(ref barcode, value);
                RaisePropertyChanged(nameof(BarcodeVisible));
                ExportCommand.RaiseCanExecuteChanged();
                PrintCommand.RaiseCanExecuteChanged();
                AddBarcodeToAppCommand.RaiseCanExecuteChanged();
            }
        }

        public bool BarcodeVisible => barcode?.Barcode != null;

        public string StatusMessage
        {
            get => statusMessage;
            set => SetProperty(ref statusMessage, value);
        }

        public GenerationDataViewModel GenerationData
        {
            get => generationData;
            set => SetProperty(ref generationData, value);
        }

        public StorageBarcodeViewModel SelectedQuickBarcode
        {
            get => selectedQuickBarcode;
            set
            {
                SetProperty(ref selectedQuickBarcode, value);
                RestoreQuickBarcodesCommand.RaiseCanExecuteChanged();
                LoadQuickBarcodesCommand.RaiseCanExecuteChanged();
                RemoveSelectedQuickBarcodeCommand.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<StorageBarcodeViewModel> QuickBarcodes
        {
            get => quickBarcodes;
            set
            {
                SetProperty(ref quickBarcodes, value);
                ClearQuickBarcodesCommand.RaiseCanExecuteChanged();
            }
        }

        private async Task RestoreBarcode(StorageBarcodeViewModel storageBarcodeViewModel)
        {
            try
            {
                var generationData = storageBarcodeViewModel.StorageBarcode.ToGenerationData();
                await HeavyAction("Generating barcode...", async () =>
                {
                    Barcode = new BarcodeViewModel(generationData)
                    {
                        Barcode = await Task.Run(() => services.GeneratorService.CreateBarcode(generationData))
                    };
                    BarcodeHeader = storageBarcodeViewModel.Title;
                });
                StatusMessage = "Barcode loaded successfully";
            }
            catch (Exception exc)
            {
                services.AppDialogsService.ShowBarcodeGenerationException(exc);
            }
        }

        private async Task GenerateBarcode(bool updateQuickBarcodes)
        {
            try
            {
                if (GenerationData.GenerateValidation())
                {
                    await HeavyAction("Generating barcode...", async () =>
                    {
                        Barcode = await GenerationData.RunGenerator();
                        BarcodeHeader = Barcode.GenerationData.GetTitle();
                        if (updateQuickBarcodes)
                        {
                            services.AppSettingsService.TryUpdateGenerationSettings(Barcode.GenerationData.ToSettings());
                            services.StorageService.AddQuickBarcode(Barcode.GenerationData.ToStorageBarcode(),
                                services.AppSettingsService.QuickBarcodesCount);
                            LoadQuickBarcodes();
                            NotifyOtherQuickGenerators();
                        }
                    });
                    StatusMessage = "Barcode generated successfully";
                }
            }
            catch (Exception exc)
            {
                services.AppDialogsService.ShowBarcodeGenerationException(exc);
            }
        }

        private void LoadQuickBarcodes()
        {
            var barcodes = new ObservableCollection<StorageBarcodeViewModel>()
            {
                emptyQuickBarcode
            };
            var fromStorage = services.StorageService.LoadQuickBarcodes();
            if (fromStorage != null)
            {
                barcodes.AddRange(fromStorage.ConvertAll(s => new StorageBarcodeViewModel(s)));
            }

            var previouslySelected = SelectedQuickBarcode;
            QuickBarcodes = barcodes;
            var toSelect = QuickBarcodes.FirstOrDefault(b => b.Compare(previouslySelected));
            selectedQuickBarcode = toSelect != null ? toSelect : quickBarcodes.First();
            RaisePropertyChanged(nameof(SelectedQuickBarcode));
        }

        private void LoadSettings()
        {
            var generationSettings = services.AppSettingsService.AppSettings.GenerationSettings;
            GenerationData.FromSettings(generationSettings);
            GenerationData.Data = string.Empty;
        }

        private void AppEvents_QuickBarcodeUpdate(QuickGeneratorViewModel sender)
        {
            if (sender != this)
            {
                LoadQuickBarcodes();
            }
        }

        private void NotifyOtherQuickGenerators()
        {
            services.AppEvents.RiseQuickBarcodeUpdate(this);
        }

        public bool OnClosing()
        {
            services.AppEvents.QuickBarcodeUpdate -= AppEvents_QuickBarcodeUpdate;
            return false;
        }
    }
}