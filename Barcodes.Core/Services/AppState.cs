using Barcodes.Core.ViewModels;
using Barcodes.Extensions;
using Barcodes.Services.Generator;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace Barcodes.Core.Services
{
    public class AppState : BindableBase, IAppState
    {
        private string statusMessage = string.Empty;
        private BarcodeResultViewModel selectedBarcode;
        private ObservableCollection<BarcodeResultViewModel> barcodes;
        private string busyMessage = string.Empty;

        private readonly IGeneratorService generatorService;

        public AppState(IGeneratorService generatorService)
        {
            this.generatorService = generatorService;

            Barcodes = new ObservableCollection<BarcodeResultViewModel>();
        }

        public bool IsBusy { get; set; }

        public string BusyMessage
        {
            get => busyMessage;
            set
            {
                SetProperty(ref busyMessage, value);
                IsBusy = !string.IsNullOrEmpty(busyMessage);
                RaisePropertyChanged(nameof(IsBusy));
            }
        }

        public string StatusMessage
        {
            get => statusMessage;
            set => SetProperty(ref statusMessage, value);
        }

        public int BarcodesCount
        {
            get => Barcodes.Count;
        }

        public BarcodeResultViewModel SelectedBarcode
        {
            get => selectedBarcode;
            set => SetProperty(ref selectedBarcode, value);
        }

        public ObservableCollection<BarcodeResultViewModel> Barcodes
        {
            get => barcodes;
            set => SetProperty(ref barcodes, value);
        }

        public void GenerateAndInsertBarcode(GenerationData barcodeData, string title)
        {
            var newBarcode = GenerateBarcode(barcodeData, title);
            InsertNewBarcode(newBarcode);
        }

        public void InsertNewBarcode(BarcodeResultViewModel barcode)
        {
            Barcodes.Insert(0, barcode);
            SetMessageAndCounter($"Barcode \"{barcode.Title}\" generated successfully!");
        }

        public void ReplaceBarcode(BarcodeResultViewModel barcode, BarcodeResultViewModel newBarcode)
        {
            var barcodeIndex = Barcodes.IndexOf(barcode);
            Barcodes.Remove(barcode);
            Barcodes.Insert(barcodeIndex, newBarcode);
            StatusMessage = $"Barcode \"{newBarcode.Title}\" edited successfully!";
        }

        public void RemoveBarcode(BarcodeResultViewModel barcode)
        {
            Barcodes.Remove(barcode);
            SetMessageAndCounter($"Successfully removed \"{barcode.Title}\"");
        }

        public void MoveDown(BarcodeResultViewModel barcode)
        {
            var index = Barcodes.IndexOf(barcode);
            Barcodes.ShiftRight(index);
        }

        public void MoveUp(BarcodeResultViewModel barcode)
        {
            var index = Barcodes.IndexOf(barcode);
            Barcodes.ShiftLeft(index);
        }

        public BarcodeResultViewModel GenerateBarcode(GenerationData barcodeData, string title)
        {
            var barcodeBitmap = generatorService.CreateBarcode(barcodeData);
            barcodeBitmap.Freeze();
            return new BarcodeResultViewModel(barcodeData)
            {
                Barcode = barcodeBitmap,
                Title = title
            };
        }

        public void SetMessageAndCounter(string message)
        {
            StatusMessage = message;
            RaisePropertyChanged(nameof(BarcodesCount));
        }
    }
}
