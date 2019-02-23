using System.Linq;
using System.Windows;

namespace Barcodes.Services.Windows
{
    public class WindowsService : IWindowsService
    {
        public bool? Show<T>(object dataContext, Window owner = null, bool modal = false)
            where T : Window, new()
        {
            var window = new T();
            return Show(window, dataContext, owner, modal);
        }

        public bool? Show(Window window, object dataContext, Window owner = null, bool modal = false)
        {
            if (dataContext != null)
            {
                window.DataContext = dataContext;
            }

            if (owner != null)
            {
                window.Owner = owner;
            }

            if (modal)
            {
                return window.ShowDialog();
            }
            else
            {
                window.Show();
                return true;
            }
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

        public Window GetActiveWindow()
        {
            return Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        }
    }
}
