using System.Windows;

namespace Barcodes.Services.Windows
{
    public interface IWindowsService
    {
        bool? Show<T>(object dataContext, Window owner = null, bool modal = false) where T : Window, new();
        bool? Show(Window window, object dataContext, Window owner = null, bool modal = false);
        bool IsWindowOpen<T>() where T : Window;
        void RestoreWindows<T>() where T : Window;
        Window GetActiveWindow();
    }
}
