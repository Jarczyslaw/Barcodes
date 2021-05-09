using Barcodes.Services.AppSettings;

namespace Barcodes.Core.Models
{
    public class SettingsSaveResult
    {
        public AppSettings Previous { get; set; }
        public AppSettings Current { get; set; }
        public bool InitialLoad { get; set; }
        public bool StoragePathChanged => Current.StoragePath != Previous.StoragePath;
        public bool BarcodesVisibleChanged => Current.BarcodesVisible != Previous.BarcodesVisible;
        public bool DragDropModeChanged => Current.DragDropMode != Previous.DragDropMode;
    }
}