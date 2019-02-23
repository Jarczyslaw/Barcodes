using Barcodes.Core.ViewModels;
using Barcodes.Services.Generator;
using System.Collections.ObjectModel;

namespace Barcodes.Core.Services
{
    public interface IAppState
    {
        bool IsBusy { get; set; }
        string BusyMessage { get; set; }
        string StatusMessage { get; set; }
        int BarcodesCount { get; }
        BarcodeResultViewModel SelectedBarcode { get; set; }
        ObservableCollection<BarcodeResultViewModel> Barcodes { get; set; }

        void GenerateAndInsertBarcode(GenerationData barcodeData, string title);
        void InsertNewBarcode(BarcodeResultViewModel barcode);
        void ReplaceBarcode(BarcodeResultViewModel barcode, BarcodeResultViewModel newBarcode);
        void RemoveBarcode(BarcodeResultViewModel barcode);
        void MoveDown(BarcodeResultViewModel barcode);
        void MoveUp(BarcodeResultViewModel barcode);
        BarcodeResultViewModel GenerateBarcode(GenerationData barcodeData, string title);

        void SetMessageAndCounter(string message);
    }
}
