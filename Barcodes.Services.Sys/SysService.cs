using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Barcodes.Services.Sys
{
    public class SysService : ISysService
    {
        public void CopyToClipboard(BitmapSource bitmapSource)
        {
            Clipboard.SetImage(bitmapSource);
        }

        public void CopyToClipboard(string data)
        {
            Clipboard.SetText(data);
        }

        public void OpenAppLocation()
        {
            var appLocation = System.AppDomain.CurrentDomain.BaseDirectory;
            StartProcess(appLocation);
        }

        public void OpenFileLocation(string filePath)
        {
            var argument = "/select, \"" + filePath + "\"";
            StartProcess("explorer.exe", argument);
        }

        public void StartProcess(string process, string arguments = null)
        {
            Process.Start(process, arguments);
        }
    }
}