using Barcodes.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Barcodes.Startup
{
    public class GlobalExceptionHandler
    {
        private readonly IDialogsService _dialogsService;

        public GlobalExceptionHandler(IDialogsService dialogsService)
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
