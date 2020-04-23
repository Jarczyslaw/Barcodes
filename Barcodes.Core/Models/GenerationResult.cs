using Barcodes.Core.ViewModels;

namespace Barcodes.Core.Models
{
    public class GenerationResult
    {
        public BarcodeViewModel Barcode { get; set; }
        public bool AddNew { get; set; }
    }
}
