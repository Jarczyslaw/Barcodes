using Barcodes.Core.Services;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Barcodes.Startup
{
    public abstract class GlobalExceptionHandler
    {
        public void Register()
        {
            AppDomain.CurrentDomain.UnhandledException +=
                (s, e) => HandleException("AppDomain.CurrentDomain.UnhandledException", (Exception)e.ExceptionObject);
            Application.Current.DispatcherUnhandledException +=
                (s, e) => e.Handled = HandleException("Application.Current.DispatcherUnhandledException", e.Exception);
            TaskScheduler.UnobservedTaskException +=
                (s, e) => HandleException("TaskScheduler.UnobservedTaskException", e.Exception);
        }

        public abstract bool HandleException(string source, Exception exception);
    }
}
