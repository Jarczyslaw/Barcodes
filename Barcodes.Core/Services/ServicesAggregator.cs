using Barcodes.Core.Abstraction;
using Barcodes.Services.AppSettings;
using Barcodes.Services.DocExport;
using Barcodes.Services.Generator;
using Barcodes.Services.Logging;
using Barcodes.Services.Storage;
using Barcodes.Services.Sys;
using Prism.Events;
using Prism.Ioc;
using System;

namespace Barcodes.Core.Services
{
    public class ServicesAggregator : IServicesAggregator
    {
        public IEventAggregator EventAggregator { get; }
        public IGeneratorService GeneratorService { get; }
        public IAppDialogsService AppDialogsService { get; }
        public IAppWindowsService AppWindowsService { get; }
        public ISysService SysService { get; }
        public IAppSettingsService AppSettingsService { get; }
        public IStorageService StorageService { get; }
        public IDocExportService DocExportService { get; }
        public IStateSaverService StateSaverService { get; }

        public IContainerExtension ContainerExtension { get; }
        public ILoggerService LoggerService { get; }

        public ServicesAggregator(IEventAggregator eventAggregator, IGeneratorService generatorService, IAppDialogsService appDialogsService,
            IAppWindowsService appWindowsService, ISysService systemService, IAppSettingsService appSettingsService,
            IStorageService storageService, IDocExportService docExportService, IStateSaverService stateSaverService,
            IContainerExtension containerExtension, ILoggerService loggerService)
        {
            EventAggregator = eventAggregator;
            GeneratorService = generatorService;
            AppDialogsService = appDialogsService;
            AppWindowsService = appWindowsService;
            SysService = systemService;
            AppSettingsService = appSettingsService;
            StorageService = storageService;
            DocExportService = docExportService;
            StateSaverService = stateSaverService;
            ContainerExtension = containerExtension;
            LoggerService = loggerService;
        }

        public void LogException(string message, Exception exc)
        {
            LoggerService.Error(exc, message);
            AppDialogsService.ShowException(message, exc);
        }

        public void OpenAppLocation()
        {
            try
            {
                SysService.OpenAppLocation();
            }
            catch (Exception exc)
            {
                LogException("Can not open app location", exc);
            }
        }
    }
}