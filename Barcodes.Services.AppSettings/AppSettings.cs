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
        public bool OpenQuickGeneratorOnStartup { get; set; }
        public int QuickBarcodesCount { get; set; } = 10;
        public string StoragePath { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"storage.{FileExtensions.Storage}");
        public bool BarcodesVisible { get; set; } = true;
        public string AntiKeyProtection { get; set; } = "F5";

        [JsonConverter(typeof(StringEnumConverter))]
        public AddMode BarcodeAddMode { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AddMode WorkspaceAddMode { get; set; }

        public bool UpdateAfterEveryGeneration { get; set; } = true;

        public GenerationSettings GenerationSettings { get; set; } = new GenerationSettings();

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