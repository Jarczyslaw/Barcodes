using System;
using System.Windows;
using System.Windows.Threading;

namespace JToolbox.WPF.Core
{
    public static class Threading
    {
        public static void SafeInvoke(Action action, DispatcherPriority priority = DispatcherPriority.Send)
        {
            if (Application.Current.Dispatcher.CheckAccess())
            {
                action.Invoke();
            }
            else
            {
                Application.Current.Dispatcher.Invoke(priority, new Action(() => action.Invoke()));
            }
        }

        public static void SafeBeginInvoke(Action action)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => action.Invoke()));
        }
    }
}