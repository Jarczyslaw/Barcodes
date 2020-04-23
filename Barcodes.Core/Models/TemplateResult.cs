using Barcodes.Services.Generator;

namespace Barcodes.Core.Models
{
    public class TemplateResult
    {
        public string Data { get; set; }
        public BarcodeType? BarcodeType { get; set; }
    }
}