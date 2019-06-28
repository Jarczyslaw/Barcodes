using Barcodes.Core.Common;
using System;
using System.IO;

namespace Barcodes.Services.AppSettings
{
    public class AppSettings
    {
        public string StoragePath { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"storage.{FileExtensions.Storage}");
        public bool BarcodesVisible { get; set; } = true;
        public string AntiKeyProtection { get; set; } = "F5";
        public GenerationSettings GenerationSettings { get; set; } = new GenerationSettings();
    }
}
