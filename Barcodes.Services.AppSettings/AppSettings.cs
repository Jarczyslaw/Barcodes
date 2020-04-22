using Barcodes.Core.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO;

namespace Barcodes.Services.AppSettings
{
    public class AppSettings
    {
        public string StoragePath { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"storage.{FileExtensions.Storage}");
        public bool BarcodesVisible { get; set; } = true;
        public string AntiKeyProtection { get; set; } = "F5";

        [JsonConverter(typeof(StringEnumConverter))]
        public AddMode BarcodeAddMode { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AddMode WorkspaceAddMode { get; set; }

        public GenerationSettings GenerationSettings { get; set; } = new GenerationSettings();
    }
}