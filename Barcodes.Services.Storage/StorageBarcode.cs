using Barcodes.Codes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Barcodes.Services.Storage
{
    public class StorageBarcode : IEquatable<StorageBarcode>
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

        public override bool Equals(object obj)
        {
            if (!(obj is StorageBarcode otherStorageBarcode))
            {
                return false;
            }

            return Equals(otherStorageBarcode);
        }

        public bool Equals(StorageBarcode other)
        {
            return Title == other.Title
                && Data == other.Data
                && Width == other.Width
                && Height == other.Height
                && DefaultSize == other.DefaultSize
                && Type == other.Type;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (17 * Title.GetHashCode()) + Data.GetHashCode() + Width.GetHashCode() + Height.GetHashCode() + DefaultSize.GetHashCode();
            }
        }
    }
}