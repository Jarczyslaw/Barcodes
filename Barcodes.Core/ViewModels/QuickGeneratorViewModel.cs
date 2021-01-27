using Barcodes.Core.Abstraction;
using Barcodes.Core.Common;
using Barcodes.Core.Extensions;
using Barcodes.Extensions;
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

        private GenerationDataViewModel generationData;
        private BarcodeViewModel barcode;
        private string statusMessage;
        private StorageBarcodeViewModel selectedQuickBarcode;
        private ObservableCollection<StorageBarcodeViewModel> quickBarcodes;
        private StorageBarcodeViewModel emptyQuickBarcode = new StorageBarcodeViewModel(null);

        public QuickGeneratorViewModel(IServicesAggregator services)
        {
            this.services = services;

            LoadQuickBarcodesCommand = new DelegateCommand(async () => await LoadBarcode(SelectedQuickBarcode),
                () => SelectedQuickBarcode?.StorageBarcode != null);
            ClearQuickBarcodesCommand = new DelegateCommand(() =>
            {
                if (services.AppDialogsService.ShowYesNoQuestion("Do you really want to clear previously generated barcodes?"))
                {
                    services.StorageService.ClearQuickBarcodes();
                    LoadQuickBarcodes();
                    NotifyOtherQuickGenerators();
                }
            }, () => QuickBarcodes?.Count > 1);

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

        public DelegateCommand CloseCommand => new DelegateCommand(() => OnClose?.Invoke());

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

        public DelegateCommand NewWindowCommand => new DelegateCommand(services.AppWindowsService.ShowQuickGeneratorWindow);

        public DelegateCommand CloseAllCommand => new DelegateCommand(services.AppWindowsService.CloseQuickGeneratorsWindows);

        public DelegateCommand ExportImageCommand => new DelegateCommand(() =>
        {
            if (CheckGeneratedBarcode())
            {
                try
                {
                    var filePath = services.AppDialogsService.SavePngFile(barcode.Title);
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
            }
        });

        public DelegateCommand ExportBarcodeCommand => new DelegateCommand(() =>
        {
            if (CheckGeneratedBarcode())
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
            }
        });

        public DelegateCommand CopyImageCommand => new DelegateCommand(() =>
        {
            if (CheckGeneratedBarcode())
            {
                services.SysService.CopyToClipboard(Barcode.Barcode);
                StatusMessage = "Barcodes copied to clipboard";
            }
        });

        public DelegateCommand ClearQuickBarcodesCommand { get; }

        public DelegateCommand LoadQuickBarcodesCommand { get; }

        public BarcodeViewModel Barcode
        {
            get => barcode;
            set
            {
                SetProperty(ref barcode, value);
                RaisePropertyChanged(nameof(BarcodeVisible));
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
                LoadQuickBarcodesCommand.RaiseCanExecuteChanged();
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

        private async Task LoadBarcode(StorageBarcodeViewModel storageBarcodeViewModel)
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
                });
                StatusMessage = $"Barcode {storageBarcodeViewModel.Title} loaded successfully";
            }
            catch (Exception exc)
            {
                services.AppDialogsService.ShowBarcodeGenerationException(exc);
            }
        }

        private async Task GenerateBarcode(bool updateQuickBarcodes)
        {
            if (GenerationData.GenerateValidation())
            {
                try
                {
                    StorageBarcodeViewModel storageBarcodeViewModel = null;
                    await HeavyAction("Generating barcode...", async () =>
                    {
                        Barcode = await GenerationData.RunGenerator();
                        storageBarcodeViewModel = new StorageBarcodeViewModel(Barcode.ToStorage());
                        if (updateQuickBarcodes)
                        {
                            services.AppSettingsService.TryUpdateGenerationSettings(GenerationData.ToSettings());
                            services.StorageService.AddQuickBarcode(storageBarcodeViewModel.StorageBarcode, 5);
                            LoadQuickBarcodes();
                            NotifyOtherQuickGenerators();
                        }
                    });
                    StatusMessage = $"Barcode {storageBarcodeViewModel.Title} generated successfully";
                }
                catch (Exception exc)
                {
                    services.AppDialogsService.ShowBarcodeGenerationException(exc);
                }
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

        private bool CheckGeneratedBarcode()
        {
            if (Barcode == null || Barcode.Barcode == null)
            {
                services.AppDialogsService.ShowError("No barcode generated");
                return false;
            }
            return true;
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