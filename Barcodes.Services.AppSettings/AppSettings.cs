using Barcodes.Core.Common;
using Barcodes.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO;

namespace Barcodes.Services.AppSettings
{
    public class AppSettings
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public StartupMode StartupMode { get; set; } = StartupMode.AddNew;

        public int QuickBarcodesCount { get; set; } = 10;
        public string StoragePath { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"storage.{FileExtensions.Storage}");
        public bool BarcodesVisible { get; set; } = true;
        public bool DescriptionVisible { get; set; }
        public string AntiKeyProtection { get; set; } = "F5";

        [JsonConverter(typeof(StringEnumConverter))]
        public AddMode BarcodeAddMode { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AddMode WorkspaceAddMode { get; set; }

        public bool UpdateAfterEveryGeneration { get; set; } = true;

        public GenerationSettings GenerationSettings { get; set; } = new GenerationSettings();

        [JsonConverter(typeof(StringEnumConverter))]
        public DragDropMode DragDropMode { get; set; } = DragDropMode.Arrangement;

        public string Serialize()
        {
            return Serializer.ToString(this);
        }

        public static AppSettings Deserialize(string serialized)
        {
            return Serializer.FromString<AppSettings>(serialized);
        }

        public string Validate()
        {
            if (StoragePath.IndexOfAny(Path.GetInvalidPathChars()) != -1)
            {
                return "Invalid storage path";
            }

            if (!KeyParser.Validate(AntiKeyProtection))
            {
                return "Invalid protected keys value";
            }

            return string.Empty;
        }
    }
}