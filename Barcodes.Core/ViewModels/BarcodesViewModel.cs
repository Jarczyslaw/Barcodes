using Aspose.BarCode.Generation;
using Barcodes.Core.Services;
using Barcodes.Services.Dialogs;
using Barcodes.Services.Generator;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Barcodes.Core.ViewModels
{
    public class BarcodesViewModel : BindableBase
    {
        private string statusMessage = string.Empty;
        public string StatusMessage
        {
            get => statusMessage;
            set => SetProperty(ref statusMessage, value);
        }

        private string data;
        public string Data
        {
            get => data;
            set => SetProperty(ref data, value);
        }

        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public int BarcodesCount
        {
            get => Barcodes.Count;
        }

        private BarcodeTypeViewModel selectedBarcodeType;
        public BarcodeTypeViewModel SelectedBarcodeType
        {
            get => selectedBarcodeType;
            set => SetProperty(ref selectedBarcodeType, value);
        }

        private BarcodeResultViewModel selectedBarcode;
        public BarcodeResultViewModel SelectedBarcode
        {
            get => selectedBarcode;
            set => SetProperty(ref selectedBarcode, value);
        }

        public ObservableCollection<BarcodeTypeViewModel> BarcodeTypes { get; private set; }
        public ObservableCollection<BarcodeResultViewModel> Barcodes { get; private set; } = new ObservableCollection<BarcodeResultViewModel>();

        private readonly IBarcodesGeneratorService barcodesGenerator;
        private readonly IDialogsService dialogsService;
        private readonly IBarcodeWindowsService barcodeWindowsService;

        public BarcodesViewModel(IBarcodesGeneratorService barcodesGenerator, IDialogsService dialogsService, IBarcodeWindowsService barcodeWindowsService)
        {
            this.barcodesGenerator = barcodesGenerator;
            this.dialogsService = dialogsService;
            this.barcodeWindowsService = barcodeWindowsService;

            InitializeBarcodeTypes();

            Data = "Test data";
            Title = "Test title";
        }

        private void InitializeBarcodeTypes()
        {
            BarcodeTypes = new ObservableCollection<BarcodeTypeViewModel>
            {
                new BarcodeTypeViewModel
                {
                    TypeTitle = "Ean13",
                    Type = EncodeTypes.EAN13
                },
                new BarcodeTypeViewModel
                {
                    TypeTitle = "Ean128",
                    Type = EncodeTypes.GS1Code128
                },
                new BarcodeTypeViewModel
                {
                    TypeTitle = "Code128",
                    Type = EncodeTypes.Code128
                },
                new BarcodeTypeViewModel
                {
                    TypeTitle = "DataMatrix",
                    Type = EncodeTypes.DataMatrix,
                    ExtraInput = barcodeWindowsService.OpenNmvsInputWindow
                },
                new BarcodeTypeViewModel
                {
                    TypeTitle = "QR",
                    Type = EncodeTypes.QR
                },
            };
            SelectedBarcodeType = BarcodeTypes.First();
        }

        private bool GenerateValidation()
        {
            if (string.IsNullOrEmpty(Title))
            {
                dialogsService.ShowError("Enter barcode's title");
                return false;
            }

            if (string.IsNullOrEmpty(Data))
            {
                dialogsService.ShowError("Enter barcode's data");
                return false;
            }

            return true;
        }

        public void GenerateBarcode()
        {
            if (!GenerateValidation())
                return;

            var barcodeData = new BarcodeData
            {
                MinWidth = 300,
                Data = Data,
                ShowData = false,
                Type = SelectedBarcodeType.Type
            };

            try
            {
                var barcode = barcodesGenerator.CreateBarcode(barcodeData);
                Barcodes.Add(new BarcodeResultViewModel
                {
                    Barcode = barcode,
                    Data = barcodeData.Data,
                    Title = Title,
                    TypeTitle = SelectedBarcodeType.TypeTitle
                });
                StatusMessage = $"Barcode \"{Title}\" generate successfully!";
                RaisePropertyChanged(nameof(BarcodesCount));
            }
            catch (Exception exc)
            {
                dialogsService.ShowException("Exception during barcode generation", exc);
            }
        }

        public void Delete(BarcodeResultViewModel barcode)
        {
            if (barcode == null)
                return;

            if (!dialogsService.ShowYesNoQuestion($"Do you really want to delete barcode \"{barcode.Title}?\""))
                return;

            Barcodes.Remove(barcode);
            RaisePropertyChanged(nameof(BarcodesCount));
        }
    }
}
