using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Barcodes.Core.UI.Services
{
    public class WindowManager
    {
        private readonly List<Window> windows = new List<Window>();
        private Window currentActiveWindow;

        public int WindowsCount => windows.Count;

        public void CloseAll()
        {
            foreach (var window in new List<Window>(windows))
            {
                window.Close();
            }
        }

        public void RegisterWindow(Window window, bool setPosition = false, bool setSize = false)
        {
            if (setSize)
            {
                SetSize(window);
            }

            if (setPosition)
            {
                SetPosition(window);
            }

            windows.Add(window);
            currentActiveWindow = window;
            window.Closed += Window_Closed;
            window.Activated += Window_Activated;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            var window = sender as Window;
            window.Closed -= Window_Closed;
            window.Activated -= Window_Activated;
            windows.Remove(window);
            if (currentActiveWindow == window)
            {
                if (windows.Count == 0)
                {
                    currentActiveWindow = null;
                }
                else
                {
                    currentActiveWindow = windows.Last();
                }
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            currentActiveWindow = sender as Window;
        }

        private void SetPosition(Window window)
        {
            if (currentActiveWindow != null)
            {
                window.Top = currentActiveWindow.Top + SystemParameters.WindowCaptionHeight;
                window.Left = currentActiveWindow.Left;
            }
        }

        private void SetSize(Window window)
        {
            if (currentActiveWindow != null)

            {
                window.Width = currentActiveWindow.Width;
                window.Height = currentActiveWindow.Height;
            }
        }
    }
}