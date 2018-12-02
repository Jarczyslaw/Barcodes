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

        public bool ExtraInputEnabled
        {
            get
            {
                if (SelectedBarcodeType == null)
                    return false;
                return SelectedBarcodeType.ExtraInput != null;
            }
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

        public ObservableCollection<BarcodeTypeViewModel> BarcodeTypes { get; private set; } = new ObservableCollection<BarcodeTypeViewModel>();
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

            Title = "Barcode1";
            Data = "Data1";
        }

        private void InitializeBarcodeTypes()
        {
            BarcodeTypes.AddRange(new ObservableCollection<BarcodeTypeViewModel>
            {
                new BarcodeTypeViewModel
                {
                    Type = BarcodeType.Ean13
                },
                new BarcodeTypeViewModel
                {
                    Type = BarcodeType.Ean128
                },
                new BarcodeTypeViewModel
                {
                    Type = BarcodeType.Code128
                },
                new BarcodeTypeViewModel
                {
                    Type = BarcodeType.DataMatrix,
                    ExtraInput = barcodeWindowsService.OpenNmvsInputWindow
                },
                new BarcodeTypeViewModel
                {
                    Type = BarcodeType.QRCode
                },
            });
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
                Data = Data,
                Type = SelectedBarcodeType.Type
            };

            try
            {
                GenerateBarcode(barcodeData, Title);
                StatusMessage = $"Barcode \"{Title}\" generate successfully!";
                RaisePropertyChanged(nameof(BarcodesCount));
            }
            catch (Exception exc)
            {
                dialogsService.ShowException("Exception during barcode generation", exc);
            }
        }

        public void GenerateBarcode(BarcodeData barcodeData, string title)
        {
            var barcode = barcodesGenerator.CreateBarcode(barcodeData);
            Barcodes.Add(new BarcodeResultViewModel
            {
                Barcode = barcode,
                Data = barcodeData.Data,
                Title = title,
                Type = barcodeData.Type,
            });
            RaisePropertyChanged(nameof(BarcodesCount));
        }

        public void Delete(BarcodeResultViewModel barcode)
        {
            if (barcode == null)
                return;

            if (!dialogsService.ShowYesNoQuestion($"Do you really want to delete barcode \"{barcode.Title}?\""))
                return;

            Barcodes.Remove(barcode);
            StatusMessage = $"Successfully removed \"{barcode.Title}\"";
            RaisePropertyChanged(nameof(BarcodesCount));
        }

        public void ExtraInput()
        {
            var result = SelectedBarcodeType.ExtraInput();
            if (string.IsNullOrEmpty(result))
                return;

            Data = result;
        }
    }
}
