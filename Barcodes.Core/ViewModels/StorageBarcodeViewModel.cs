using Barcodes.Services.Storage;

namespace Barcodes.Core.ViewModels
{
    public class StorageBarcodeViewModel
    {
        public StorageBarcodeViewModel(StorageBarcode storageBarcode)
        {
            StorageBarcode = storageBarcode;
        }

        public StorageBarcode StorageBarcode { get; set; }

        public string Title
        {
            get => $"{StorageBarcode.Data} - {StorageBarcode.Type}";
        }
    }
}