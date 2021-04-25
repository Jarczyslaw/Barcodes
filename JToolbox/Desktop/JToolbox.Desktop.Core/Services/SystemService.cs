using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media.Imaging;

namespace JToolbox.Desktop.Core.Services
{
    public class SystemService : ISystemService
    {
        public void CopyToClipboard(BitmapSource bitmapSource)
        {
            Clipboard.SetImage(bitmapSource);
        }

        public void CopyToClipboard(string data)
        {
            Clipboard.SetText(data);
        }

        public string GetTextFromClipboard()
        {
            try
            {
                return Clipboard.GetText();
            }
            catch
            {
                return null;
            }
        }

        public void OpenAppLocation()
        {
            var appLocation = AppDomain.CurrentDomain.BaseDirectory;
            OpenFolderLocation(appLocation);
        }

        public void OpenFolderLocation(string folderPath)
        {
            StartProcess(folderPath);
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

        public void StartProcessSilent(string process, string arguments = null)
        {
            var processStart = new ProcessStartInfo(process, arguments)
            {
                CreateNoWindow = true,
                UseShellExecute = false
            };
            Process.Start(processStart);
        }

        public void Shutdown()
        {
            StartProcessSilent("shutdown", "-s -t 0");
        }

        public void Restart()
        {
            StartProcessSilent("shutdown", "-r -t 0");
        }
    }
}