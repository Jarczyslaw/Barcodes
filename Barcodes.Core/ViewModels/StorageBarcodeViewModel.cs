using Barcodes.Services.Storage;
using System.Text.RegularExpressions;

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
                return $"{GetSanitizedData()} - {StorageBarcode.Type}";
            }
        }

        public bool Compare(StorageBarcodeViewModel other)
        {
            return other.StorageBarcode.Data == StorageBarcode.Data && other.StorageBarcode.Type == StorageBarcode.Type;
        }

        private string GetSanitizedData()
        {
            var result = Regex.Replace(StorageBarcode.Data, @"\t|\n|\r", " ");
            if (result.Length > 30)
            {
                result = result.Substring(0, 27) + "...";
            }
            return result;
        }
    }
}