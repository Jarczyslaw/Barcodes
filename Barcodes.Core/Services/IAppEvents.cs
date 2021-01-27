using Barcodes.Core.Models;
using Barcodes.Core.ViewModels;

namespace Barcodes.Core.Services
{
    public interface IAppEvents
    {
        event QuickBarcodeGenerated QuickBarcodeUpdate;
        event SettingsChanged SettingsChanged;

        void RiseQuickBarcodeUpdate(QuickGeneratorViewModel sender);
        void RiseSettingsChanged(SettingsSaveResult settingsSaveResult);
    }
}