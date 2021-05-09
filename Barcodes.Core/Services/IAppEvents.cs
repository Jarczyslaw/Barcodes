using Barcodes.Core.Models;
using Barcodes.Core.ViewModels;
using Barcodes.Services.AppSettings;

namespace Barcodes.Core.Services
{
    public interface IAppEvents
    {
        event QuickBarcodeGenerated OnQuickBarcodeUpdate;

        event SettingsChanged OnSettingsChanged;

        event DragDropModeChanged OnDragDropModeChanged;

        void RiseOnQuickBarcodeUpdate(QuickGeneratorViewModel sender);

        void RiseOnSettingsChanged(SettingsSaveResult settingsSaveResult);

        void RiseOnDragDropModeChanged(DragDropMode mode);
    }
}