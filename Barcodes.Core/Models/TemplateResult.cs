using Barcodes.Codes;

namespace Barcodes.Core.Models
{
    public class TemplateResult
    {
        public TemplateResult()
        {
        }

        public TemplateResult(BaseCode baseCode)
        {
            Data = baseCode.Code;
            BarcodeType = baseCode.Type;
        }

        public string Data { get; set; }
        public BarcodeType? BarcodeType { get; set; }
    }
}