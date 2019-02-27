using System;
using Barcodes.Core.Services;

namespace Barcodes.Startup
{
    public class AppGlobalExceptionHandler : GlobalExceptionHandler
    {
        private readonly IAppDialogsService appDialogsService;

        public AppGlobalExceptionHandler(IAppDialogsService appDialogsService)
        {
            this.appDialogsService = appDialogsService;
        }

        public override bool HandleException(string source, Exception exception)
        {
            var message = $"Unexpected critical exception - {source}";
            appDialogsService.ShowCriticalException(message, exception);
            return true;
        }
    }
}
