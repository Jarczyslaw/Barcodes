using Barcodes.Core.Models;
using Barcodes.Core.ViewModels;
using Barcodes.Services.AppSettings;

namespace Barcodes.Core.Services
{
    public delegate void SettingsChanged(SettingsSaveResult settingsSaveResult);

    public delegate void QuickBarcodeGenerated(QuickGeneratorViewModel sender);

    public delegate void DragDropModeChanged(DragDropMode mode);

    public class AppEvents : IAppEvents
    {
        public event SettingsChanged OnSettingsChanged = delegate { };

        public event QuickBarcodeGenerated OnQuickBarcodeUpdate = delegate { };

        public event DragDropModeChanged OnDragDropModeChanged = delegate { };

        public void RiseOnSettingsChanged(SettingsSaveResult settingsSaveResult)
        {
            OnSettingsChanged(settingsSaveResult);
        }

        public void RiseOnQuickBarcodeUpdate(QuickGeneratorViewModel sender)
        {
            OnQuickBarcodeUpdate(sender);
        }

        public void RiseOnDragDropModeChanged(DragDropMode mode)
        {
            OnDragDropModeChanged(mode);
        }
    }
}