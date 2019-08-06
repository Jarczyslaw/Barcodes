using Barcodes.Core.Common;
using Barcodes.Services.AppSettings;
using Barcodes.Services.Dialogs;
using Barcodes.Services.Generator;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;

namespace Barcodes.Core.ViewModels
{
    public class ExamplesViewModel : BindableBase, ICloseSource
    {
        private ObservableCollection<BarcodeViewModel> barcodes;
        private BarcodeViewModel selectedBarcode;

        public ExamplesViewModel(IGeneratorService generatorService, IDialogsService dialogsService, IAppSettingsService appSettingsService)
        {
            Barcodes = BarcodeExamples.CreateExamples(generatorService, appSettingsService.AppSettings.GenerationSettings);
            GenerateCommand = new DelegateCommand(() =>
            {
                if (SelectedBarcode == null)
                {
                    dialogsService.ShowInfo("Select example barcode");
                    return;
                }
                ResultBarcode = SelectedBarcode;
                OnClose?.Invoke();
            });
            CloseCommand = new DelegateCommand(() => OnClose?.Invoke());
        }

        public DelegateCommand GenerateCommand { get; }

        public DelegateCommand CloseCommand { get; }

        public BarcodeViewModel ResultBarcode { get; private set; }

        public ObservableCollection<BarcodeViewModel> Barcodes
        {
            get => barcodes;
            set => SetProperty(ref barcodes, value);
        }

        public BarcodeViewModel SelectedBarcode
        {
            get => selectedBarcode;
            set => SetProperty(ref selectedBarcode, value);
        }

        public Action OnClose { get; set; }
    }
}