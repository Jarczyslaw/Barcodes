using Barcodes.Codes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Barcodes.Services.AppSettings
{
    public class GenerationSettings
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public BarcodeType Type { get; set; } = BarcodeType.DataMatrix;

        public bool DefaultSize { get; set; } = true;
        public int Width { get; set; } = 100;
        public int Height { get; set; } = 100;
        public bool ValidateCodeText { get; set; } = true;
    }
}