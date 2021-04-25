using Barcodes.Core.Abstraction;
using JToolbox.Core.Abstraction;
using JToolbox.WPF.Core;
using System;

namespace Barcodes.Startup
{
    public class AppGlobalExceptionHandler : GlobalExceptionHandler
    {
        private readonly IAppDialogsService appDialogsService;
        private readonly ILoggerService loggerService;

        public AppGlobalExceptionHandler(IAppDialogsService appDialogsService, ILoggerService loggerService)
        {
            this.appDialogsService = appDialogsService;
            this.loggerService = loggerService;
        }

        public override bool HandleException(string source, Exception exception)
        {
            var message = $"Unexpected critical exception - {source}";
            loggerService.Fatal(exception, message);
            appDialogsService.ShowCriticalException(exception, message);
            return true;
        }
    }
}