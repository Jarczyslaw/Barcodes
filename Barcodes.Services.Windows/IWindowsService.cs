using System.Windows;

namespace Barcodes.Services.Windows
{
    public interface IWindowsService
    {
        void Show<T>(object dataContext) where T : Window, new();
        void Show<T>(T window, object dataContext) where T : Window;
        bool? ShowModal<T>(object dataContext) where T : Window, new();
        bool? ShowModal<T>(T window, object dataContext) where T : Window;
        bool IsWindowOpen<T>() where T : Window;
        void RestoreWindows<T>() where T : Window;
    }
}
