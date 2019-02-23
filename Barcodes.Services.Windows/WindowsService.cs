﻿using System.Linq;
using System.Windows;

namespace Barcodes.Services.Windows
{
    public class WindowsService : IWindowsService
    {
        public void Show<T>(object dataContext)
           where T : Window, new()
        {
            var window = new T();
            Show(window, dataContext);
        }

        public void Show<T>(T window, object dataContext)
            where T : Window
        {
            if (dataContext != null)
            {
                window.DataContext = dataContext;
            }
            window.Show();
        }

        public bool? ShowModal<T>(object dataContext)
            where T : Window, new()
        {
            var window = new T();
            return ShowModal(window, dataContext);
        }

        public bool? ShowModal<T>(T window, object dataContext)
            where T : Window
        {
            if (dataContext != null)
            {
                window.DataContext = dataContext;
            }
            return window.ShowDialog();
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

        private Window GetActiveWindow()
        {
            return Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
        }
    }
}
