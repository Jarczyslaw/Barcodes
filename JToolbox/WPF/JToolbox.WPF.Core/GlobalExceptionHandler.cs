using System;
using System.Threading.Tasks;
using System.Windows;

namespace JToolbox.WPF.Core
{
    public abstract class GlobalExceptionHandler
    {
        public void Register()
        {
            AppDomain.CurrentDomain.UnhandledException +=
                (s, e) => HandleException(nameof(AppDomain.CurrentDomain.UnhandledException), (Exception)e.ExceptionObject);
            Application.Current.DispatcherUnhandledException +=
                (s, e) => e.Handled = HandleException(nameof(Application.Current.DispatcherUnhandledException), e.Exception);
            TaskScheduler.UnobservedTaskException +=
                (s, e) => HandleException(nameof(TaskScheduler.UnobservedTaskException), e.Exception);
        }

        public abstract bool HandleException(string source, Exception exception);
    }
}