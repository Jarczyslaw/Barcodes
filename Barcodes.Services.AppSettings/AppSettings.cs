using System;
using System.IO;

namespace Barcodes.Services.AppSettings
{
    public class AppSettings
    {
        public string StoragePath { get; set; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "storage.json");
    }
}
