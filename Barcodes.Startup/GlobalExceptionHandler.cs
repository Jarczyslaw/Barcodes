using Barcodes.Core.Services;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Barcodes.Startup
{
    public class GlobalExceptionHandler
    {
        private readonly IAppDialogsService _dialogsService;

        public GlobalExceptionHandler(IAppDialogsService dialogsService)
        {
            _dialogsService = dialogsService;
        }

        public void RegisterEvents(Application app)
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) => LogUnhandledException((Exception)e.ExceptionObject,
                "AppDomain.CurrentDomain.UnhandledException");
            TaskScheduler.UnobservedTaskException += (s, e) => LogUnhandledException(e.Exception,
                "TaskScheduler.UnobservedTaskException");
            app.DispatcherUnhandledException += (s, e) =>
            {
                e.Handled = true;
                LogUnhandledException(e.Exception, "Application.Current.DispatcherUnhandledException");
            };
        }

        private void LogUnhandledException(Exception exception, string source)
        {
            var message = $"Unexpected critical exception - {source}";
            _dialogsService.ShowCriticalException(message, exception);
        }
    }
}
