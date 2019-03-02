using System.Linq;
using System.Windows;

namespace Barcodes.Services.Windows
{
    public class WindowsService : IWindowsService
    {
        public bool? ShowDialog(Window window, object dataContext = null)
        {
            SetupWindow(window, dataContext, GetActiveWindow());
            return window.ShowDialog();
        }

        public void Show(Window window, object dataContext = null, Window owner = null)
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

            if (owner != null)
            {
                window.Owner = owner;
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
