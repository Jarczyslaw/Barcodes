using Barcodes.Core.Abstraction;
using Barcodes.Core.Models;
using Barcodes.Extensions;
using Barcodes.Services.AppSettings;
using Barcodes.Services.Generator;
using Barcodes.Services.Storage;
using Barcodes.Services.Sys;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Barcodes.Core.ViewModels
{
    public class QuickGeneratorViewModel : BaseViewModel
    {
        private readonly IAppDialogsService appDialogsService;
        private readonly IAppSettingsService appSettingsService;
        private readonly IEventAggregator eventAggregator;
        private readonly IAppWindowsService appWindowsService;
        private readonly ISysService sysService;
        private readonly IStorageService storageService;

        private GenerationDataViewModel generationData;
        private BarcodeViewModel barcode;
        private string statusMessage;
        private StorageBarcodeViewModel selectedQuickBarcode;
        private ObservableCollection<StorageBarcodeViewModel> quickBarcodes;
        private StorageBarcodeViewModel emptyQuickBarcode = new StorageBarcodeViewModel(null);

        public QuickGeneratorViewModel(IAppDialogsService appDialogsService, IAppWindowsService appWindowsService, ISysService sysService,
            IAppSettingsService appSettingsService, IGeneratorService generatorService, IStorageService storageService,
            IEventAggregator eventAggregator)
        {
            this.appDialogsService = appDialogsService;
            this.appSettingsService = appSettingsService;
            this.eventAggregator = eventAggregator;
            this.appWindowsService = appWindowsService;
            this.sysService = sysService;
            this.storageService = storageService;

            eventAggregator.GetEvent<OnBarcodeGeneratedEvent>().Subscribe(OnBarcodeGenerated);

            generationData = new GenerationDataViewModel(appDialogsService, appWindowsService, generatorService);
            LoadSettings();
            LoadQuickBarcodes();
        }

        public DelegateCommand GenerateCommand => new DelegateCommand(() => GenerateBarcode());

        public DelegateCommand NewWindowCommand => new DelegateCommand(() => appWindowsService.ShowQuickGeneratorWindow());

        public DelegateCommand CloseAllCommand => new DelegateCommand(() => appWindowsService.CloseAllQuickGenerators());

        public DelegateCommand ExportImageCommand => new DelegateCommand(() =>
        {
            if (CheckGeneratedBarcode())
            {
                try
                {
                    var filePath = appDialogsService.SavePngFile(barcode.Title);
                    if (string.IsNullOrEmpty(filePath))
                    {
                        return;
                    }

                    barcode.Barcode.ToPng(filePath);
                    StatusMessage = $"Barcode saved in {Path.GetFileName(filePath)}";
                }
                catch (Exception exc)
                {
                    appDialogsService.ShowException("Error when saving barcode to png file", exc);
                }
            }
        });

        public DelegateCommand ExportBarcodeCommand => new DelegateCommand(() =>
        {
            if (CheckGeneratedBarcode())
            {
                var barcodeFile = appDialogsService.ExportBarcodesFile();
                if (string.IsNullOrEmpty(barcodeFile))
                {
                    return;
                }

                try
                {
                    storageService.ExportBarcodes(barcodeFile, new List<StorageBarcode> { Barcode.ToStorage() });
                    StatusMessage = "Barcode exported successfully";
                }
                catch (Exception exc)
                {
                    appDialogsService.ShowException("Error when exporting barcode", exc);
                }
            }
        });

        public DelegateCommand CopyImageCommand => new DelegateCommand(() =>
        {
            if (CheckGeneratedBarcode())
            {
                sysService.CopyToClipboard(Barcode.Barcode);
                StatusMessage = "Barcodes copied to clipboard";
            }
        });

        public DelegateCommand ClearQuickBarcodesCommand => new DelegateCommand(() =>
        {
            if (appDialogsService.ShowYesNoQuestion("Do you really want to clear previously generated barcodes?"))
            {
                storageService.ClearQuickBarcodes();
                LoadQuickBarcodes();
                eventAggregator.GetEvent<OnBarcodeGeneratedEvent>().Publish(new QuickGeneratorSuccess
                {
                    Sender = this,
                });
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
            get
            {
                return selectedQuickBarcode ?? emptyQuickBarcode;
            }
            set => SetProperty(ref selectedQuickBarcode, value);
        }

        public bool QuickBarcodesVisible
        {
            get => QuickBarcodes?.Count > 0;
        }

        public ObservableCollection<StorageBarcodeViewModel> QuickBarcodes
        {
            get
            {
                var result = new ObservableCollection<StorageBarcodeViewModel>()
                {
                    emptyQuickBarcode
                };
                if (quickBarcodes != null)
                {
                    result.AddRange(quickBarcodes);
                }
                return result;
            }
            set
            {
                SetProperty(ref quickBarcodes, value);
                RaisePropertyChanged(nameof(QuickBarcodesVisible));
            }
        }

        private async void GenerateBarcode()
        {
            if (GenerationData.GenerateValidation())
            {
                try
                {
                    IsBusy = true;
                    Barcode = await GenerationData.RunGenerator();
                    appSettingsService.TryUpdateGenerationSettings(GenerationData.ToSettings());
                    storageService.AddQuickBarcode(Barcode.ToStorage(), 5);
                    LoadQuickBarcodes();
                    eventAggregator.GetEvent<OnBarcodeGeneratedEvent>().Publish(new QuickGeneratorSuccess
                    {
                        Sender = this,
                    });
                    IsBusy = false;
                    StatusMessage = "Barcode generated successfully";
                }
                catch (Exception exc)
                {
                    appDialogsService.ShowException("Exception during barcode generation. Try disabling validation and adjust the barcode sizes", exc);
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        private void LoadQuickBarcodes()
        {
            var quickBarcodes = storageService.LoadQuickBarcodes();
            if (quickBarcodes != null)
            {
                var newBarcodes = quickBarcodes.ConvertAll(s => new StorageBarcodeViewModel(s));
                var toSelect = newBarcodes.Find(b => SelectedQuickBarcode != null && b.Title == SelectedQuickBarcode.Title);
                QuickBarcodes = new ObservableCollection<StorageBarcodeViewModel>(newBarcodes);
                SelectedQuickBarcode = toSelect;
            }
            else
            {
                SelectedQuickBarcode = null;
                QuickBarcodes = null;
                SelectedQuickBarcode = null;
            }
        }

        private void LoadSettings()
        {
            var generationSettings = appSettingsService.AppSettings.GenerationSettings;
            GenerationData.FromSettings(generationSettings);
            GenerationData.Data = string.Empty;
        }

        private void OnBarcodeGenerated(QuickGeneratorSuccess quickGeneratorSuccess)
        {
            if (quickGeneratorSuccess.Sender != this)
            {
                LoadQuickBarcodes();
            }
        }

        private bool CheckGeneratedBarcode()
        {
            if (Barcode == null || Barcode.Barcode == null)
            {
                appDialogsService.ShowError("No barcode generated");
                return false;
            }
            return true;
        }

        private class OnBarcodeGeneratedEvent : PubSubEvent<QuickGeneratorSuccess>
        {
        }
    }
}