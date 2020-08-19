using System.Linq;
using System.Windows;

namespace Barcodes.Services.Windows
{
    public class WindowsService : IWindowsService
    {
        public Window MainWindow => Application.Current.MainWindow;

        public Window ActiveWindow => Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);

        public bool? ShowDialog(Window window, Window owner, object dataContext = null)
        {
            SetupWindow(window, dataContext, owner);
            return window.ShowDialog();
        }

        public void Show(Window window, Window owner, object dataContext = null)
        {
            SetupWindow(window, dataContext, owner);
            window.Show();
        }

        private void SetupWindow(Window window, object dataContext, Window owner)
        {
            if (dataContext != null)
            {
                window.DataContext = dataContext;
            }

            window.Owner = owner;
        }

        public bool IsWindowOpen<T>()
            where T : Window
        {
            var windows = Application.Current.Windows.OfType<T>();
            return windows.Any();
        }

        public void RestoreWindows<T>()
            where T : Window
        {
            foreach (var window in Application.Current.Windows.OfType<T>())
            {
                if (window.WindowState == WindowState.Minimized)
                {
                    window.WindowState = WindowState.Normal;
                }

                window.Activate();
            }
        }
    }
}