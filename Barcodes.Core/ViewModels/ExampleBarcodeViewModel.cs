using Barcodes.Codes;
using Barcodes.Services.Generator;

namespace Barcodes.Core.ViewModels
{
    public class ExampleBarcodeViewModel : BarcodeViewModel
    {
        public ExampleBarcodeViewModel(GenerationData generationData)
            : base(generationData)
        {
        }

        public BarcodeTemplate Template { get; set; }
    }
}