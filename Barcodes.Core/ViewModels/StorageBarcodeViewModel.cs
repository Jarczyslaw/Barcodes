using Barcodes.Core.Extensions;
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
            get
            {
                if (StorageBarcode == null)
                {
                    return "Generate and select barcode";
                }
                return StorageBarcode.GetTitle();
            }
        }

        public bool Compare(StorageBarcodeViewModel other)
        {
            if (StorageBarcode == null || other?.StorageBarcode == null)
            {
                return false;
            }
            return other.StorageBarcode.Data == StorageBarcode.Data && other.StorageBarcode.Type == StorageBarcode.Type;
        }
    }
}