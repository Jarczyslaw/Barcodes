using Barcodes.Core.ViewModels;

namespace Barcodes.Core.ViewModelsResult
{
    public class GenerationViewModelResult
    {
        public BarcodeViewModel Barcode { get; set; }
        public bool AddNew { get; set; }
    }
}
