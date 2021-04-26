using System.Windows.Media.Imaging;

namespace JToolbox.Desktop.Core.Services
{
    public interface ISystemService
    {
        void CopyToClipboard(BitmapSource bitmapSource);

        void CopyToClipboard(string data);

        string GetTextFromClipboard();

        void OpenAppLocation();

        void OpenFolderLocation(string folderPath);

        void OpenFileLocation(string filePath);

        void StartProcess(string process, string arguments = null);

        void StartProcessSilent(string process, string arguments = null);

        void Shutdown();

        void Restart();
    }
}