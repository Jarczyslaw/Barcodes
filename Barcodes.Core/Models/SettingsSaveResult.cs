using Barcodes.Services.AppSettings;

namespace Barcodes.Core.Models
{
    public class SettingsSaveResult
    {
        public string PreviusStoragePath { get; set; }
        public string CurrentStoragePath { get; set; }
        public bool BarcodesVisible { get; set; }
        public DragDropMode DragDropMode { get; set; }
        public bool StoragePathChanged => PreviusStoragePath != CurrentStoragePath;
    }
}