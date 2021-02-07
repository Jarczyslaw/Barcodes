using Barcodes.Core.Abstraction;
using Barcodes.Services.AppSettings;
using Barcodes.Services.DocExport;
using Barcodes.Services.Generator;
using Barcodes.Services.Logging;
using Barcodes.Services.Storage;
using Barcodes.Services.Sys;
using Prism.Ioc;
using System;
using System.IO;

namespace Barcodes.Core.Services
{
    public class ServicesAggregator : IServicesAggregator
    {
        public IAppEvents AppEvents { get; }
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

        public ServicesAggregator(IAppEvents appEvents, IGeneratorService generatorService, IAppDialogsService appDialogsService,
            IAppWindowsService appWindowsService, ISysService systemService, IAppSettingsService appSettingsService,
            IStorageService storageService, IDocExportService docExportService, IStateSaverService stateSaverService,
            IContainerExtension containerExtension, ILoggerService loggerService)
        {
            AppEvents = appEvents;
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

        public void OpenStorageLocation(string storagePath)
        {
            const string openErrorMessage = "Can not open storage file location";
            try
            {
                if (!File.Exists(storagePath))
                {
                    AppDialogsService.ShowError(openErrorMessage);
                    return;
                }

                SysService.OpenFileLocation(storagePath);
            }
            catch (Exception exc)
            {
                LogException(openErrorMessage, exc);
            }
        }

        public void OpenLogs()
        {
            const string openErrorMessage = "Can not open logs file location";
            try
            {
                if (!File.Exists(LoggerService.LogFilePath))
                {
                    AppDialogsService.ShowError(openErrorMessage);
                    return;
                }

                SysService.StartProcess(LoggerService.LogFilePath);
            }
            catch (Exception exc)
            {
                LogException(openErrorMessage, exc);
            }
        }

        public void OpenNewFile(string filePath)
        {
            if (AppDialogsService.ShowYesNoQuestion("Do you want to open the newly generated file?"))
            {
                SysService.StartProcess(filePath);
            }
        }
    }
}