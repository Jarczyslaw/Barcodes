using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Barcodes.Services.System
{
    public class SystemService : ISystemService
    {
        public void CopyToClipboard(BitmapSource bitmapSource)
        {
            Clipboard.SetImage(bitmapSource);
        }

        public void OpenLocation(string location)
        {
            string argument = "/select, \"" + location + "\"";
            StartProcess("explorer.exe", argument);
        }

        public void StartProcess(string process, string arguments = null)
        {
            Process.Start(process, arguments);
        }
    }
}
