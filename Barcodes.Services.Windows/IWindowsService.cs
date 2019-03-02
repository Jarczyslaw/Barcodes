using System.Windows;

namespace Barcodes.Services.Windows
{
    public interface IWindowsService
    {
        bool? ShowDialog(Window window, object dataContext = null);
        void Show(Window window, object dataContext = null, Window owner = null);
        bool IsWindowOpen<T>() where T : Window;
        void RestoreWindows<T>() where T : Window;
        Window GetActiveWindow();
    }
}
