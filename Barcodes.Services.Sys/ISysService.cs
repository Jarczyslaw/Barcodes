using System.Windows.Media.Imaging;

namespace Barcodes.Services.Sys
{
    public interface ISysService
    {
        void CopyToClipboard(BitmapSource bitmapSource);

        void CopyToClipboard(string data);

        void OpenAppLocation();

        void OpenFileLocation(string filePath);

        void StartProcess(string process, string arguments = null);
    }
}