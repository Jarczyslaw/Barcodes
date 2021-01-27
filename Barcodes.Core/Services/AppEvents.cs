using Barcodes.Core.Models;
using Barcodes.Core.ViewModels;

namespace Barcodes.Core.Services
{
    public delegate void SettingsChanged(SettingsSaveResult settingsSaveResult);

    public delegate void QuickBarcodeGenerated(QuickGeneratorViewModel sender);

    public class AppEvents : IAppEvents
    {
        public event SettingsChanged SettingsChanged;
        public event QuickBarcodeGenerated QuickBarcodeUpdate;

        public void RiseSettingsChanged(SettingsSaveResult settingsSaveResult)
        {
            SettingsChanged?.Invoke(settingsSaveResult);
        }

        public void RiseQuickBarcodeUpdate(QuickGeneratorViewModel sender)
        {
            QuickBarcodeUpdate?.Invoke(sender);
        }
    }
}