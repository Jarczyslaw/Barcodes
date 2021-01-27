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
            if (StorageBarcode == null || other?.StorageBarcode == null)
            {
                return false;
            }
            return other.StorageBarcode.Data == StorageBarcode.Data && other.StorageBarcode.Type == StorageBarcode.Type;
        }

        private string GetSanitizedData()
        {
            var result = Regex.Replace(StorageBarcode.Data, @"\t|\n|\r", " ");
            var maxLength = 25;
            if (result.Length > maxLength)
            {
                result = result.Substring(0, maxLength) + "...";
            }
            return result;
        }
    }
}