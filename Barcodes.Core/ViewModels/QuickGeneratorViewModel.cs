using Barcodes.Core.Abstraction;
using Barcodes.Core.Events;
using Barcodes.Core.Extensions;
using Barcodes.Extensions;
using Barcodes.Services.Storage;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Barcodes.Core.ViewModels
{
    public class QuickGeneratorViewModel : BaseViewModel
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

            services.EventAggregator.GetEvent<OnBarcodeGeneratedEvent>().Subscribe(OnBarcodeGenerated, threadOption: ThreadOption.UIThread);

            generationData = new GenerationDataViewModel(services.AppDialogsService, services.AppWindowsService, services.GeneratorService,
                services.SysService, services.AppSettingsService);
            LoadSettings();
            LoadQuickBarcodes();
        }

        public DelegateCommand OpenAppLocationCommand => new DelegateCommand(services.OpenAppLocation);

        public DelegateCommand CloseCommand => new DelegateCommand(() => OnClose?.Invoke());

        public DelegateCommand CloseAppCommand => new DelegateCommand(services.AppWindowsService.CloseShell);

        public DelegateCommand SettingsCommand => new DelegateCommand(() =>
        {
        });

        public DelegateCommand ExamplesCommand => new DelegateCommand(() =>
        {
        });

        public DelegateCommand AboutCommand => new DelegateCommand(() =>
        {
        });

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
                if (string.IsNullOrEmpty(barcodeFile))
                {
                    return;
                }

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
        });

        public DelegateCommand CopyImageCommand => new DelegateCommand(() =>
        {
            if (CheckGeneratedBarcode())
            {
                services.SysService.CopyToClipboard(Barcode.Barcode);
                StatusMessage = "Barcodes copied to clipboard";
            }
        });

        public DelegateCommand ClearQuickBarcodesCommand => new DelegateCommand(() =>
        {
            if (services.AppDialogsService.ShowYesNoQuestion("Do you really want to clear previously generated barcodes?"))
            {
                services.StorageService.ClearQuickBarcodes();
                LoadQuickBarcodes();
                NotifyOtherQuickGenerators();
            }
        });

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
                SetProperty(ref selectedQuickBarcode, value, nameof(SelectedQuickBarcode));
                SetSelectedQuickBarcode(value, false);
            }
        }

        public ObservableCollection<StorageBarcodeViewModel> QuickBarcodes
        {
            get => quickBarcodes;
            set => SetProperty(ref quickBarcodes, value);
        }

        private async void SetSelectedQuickBarcode(StorageBarcodeViewModel value, bool updateQuickBarcodes)
        {
            if (value?.StorageBarcode != null)
            {
                GenerationData.FromData(value.StorageBarcode.ToGenerationData());
                await GenerateBarcode(updateQuickBarcodes);
            }
        }

        private async Task GenerateBarcode(bool updateQuickBarcodes)
        {
            if (GenerationData.GenerateValidation())
            {
                try
                {
                    await HeavyAction("Generating barcode...", async () =>
                    {
                        Barcode = await GenerationData.RunGenerator();
                        if (updateQuickBarcodes)
                        {
                            services.AppSettingsService.TryUpdateGenerationSettings(GenerationData.ToSettings());
                            services.StorageService.AddQuickBarcode(Barcode.ToStorage(), 5);
                            LoadQuickBarcodes();
                            NotifyOtherQuickGenerators();
                        }
                    });
                    StatusMessage = "Barcode generated successfully";
                }
                catch (Exception exc)
                {
                    services.AppDialogsService.ShowException("Exception during barcode generation. Try disabling validation and adjust the barcode sizes", exc);
                }
                finally
                {
                    IsBusy = false;
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
            var toSelect = QuickBarcodes.FirstOrDefault(b => previouslySelected != null && b.Title == previouslySelected.Title);
            selectedQuickBarcode = toSelect != null ? toSelect : quickBarcodes.First();
            RaisePropertyChanged(nameof(SelectedQuickBarcode));
        }

        private void LoadSettings()
        {
            var generationSettings = services.AppSettingsService.AppSettings.GenerationSettings;
            GenerationData.FromSettings(generationSettings);
            GenerationData.Data = string.Empty;
        }

        private void OnBarcodeGenerated(QuickGeneratorViewModel sender)
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
            services.EventAggregator.GetEvent<OnBarcodeGeneratedEvent>().Publish(this);
        }
    }
}