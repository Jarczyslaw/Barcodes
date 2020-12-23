using Barcodes.Services.AppSettings;
using Barcodes.Services.DocExport;
using Barcodes.Services.Generator;
using Barcodes.Services.Logging;
using Barcodes.Services.Storage;
using Barcodes.Services.Sys;
using Prism.Events;
using Prism.Ioc;
using System;

namespace Barcodes.Core.Abstraction
{
    public interface IServicesAggregator
    {
        IEventAggregator EventAggregator { get; }
        IGeneratorService GeneratorService { get; }
        IAppDialogsService AppDialogsService { get; }
        IAppWindowsService AppWindowsService { get; }
        ISysService SystemService { get; }
        IAppSettingsService AppSettingsService { get; }
        IStorageService StorageService { get; }
        IDocExportService DocExportService { get; }
        IStateSaverService StateSaverService { get; }
        IContainerExtension ContainerExtension { get; }
        ILoggerService LoggerService { get; }
        void LogException(string message, Exception exc);
    }
}