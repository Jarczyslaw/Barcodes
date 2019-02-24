using Barcodes.Services.Generator;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Barcodes.Services.Storage
{
    public class StorageBarcode
    {
        public string Title { get; set; }
        public string Data { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool DefaultSize { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public BarcodeType Type { get; set; }

        [JsonIgnore]
        public bool IsValid => !string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(Data);

        [JsonIgnore]
        public bool ValidSizes => DefaultSize || (Width != 0 && Height != 0);
    }
}
