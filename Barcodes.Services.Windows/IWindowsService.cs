using System.Windows;

namespace Barcodes.Services.Windows
{
    public interface IWindowsService
    {
        Window MainWindow { get; }

        Window ActiveWindow { get; }

        bool? ShowDialog(Window window, Window owner, object dataContext = null);

        void Show(Window window, Window owner, object dataContext = null);

        bool IsWindowOpen<T>() where T : Window;

        void RestoreWindows<T>() where T : Window;
    }
}