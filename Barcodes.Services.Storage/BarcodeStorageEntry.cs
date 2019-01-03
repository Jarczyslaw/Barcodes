using Barcodes.Services.Generator;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Barcodes.Services.Storage
{
    public class BarcodeStorageEntry
    {
        public string Title { get; set; }
        public string Data { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public BarcodeType Type { get; set; }
    }
}
