using Barcodes.Services.AppSettings;

namespace Barcodes.Core.ViewModels
{
    public class StartupModeViewModel
    {
        public StartupModeViewModel(StartupMode startupMode)
        {
            StartupMode = startupMode;
        }

        public StartupMode StartupMode { get; }

        public string Title
        {
            get
            {
                if (StartupMode == StartupMode.DoNothing)
                {
                    return "Do nothing";
                }
                else if (StartupMode == StartupMode.AddNew)
                {
                    return "Show new barcode window";
                }
                else if (StartupMode == StartupMode.QuickGenerator)
                {
                    return "Show quick generator";
                }
                return string.Empty;
            }
        }
    }
}