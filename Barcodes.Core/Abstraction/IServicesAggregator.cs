using Barcodes.Core.Services;
using Barcodes.Services.AppSettings;
using Barcodes.Services.DocExport;
using Barcodes.Services.Generator;
using Barcodes.Services.Storage;
using JToolbox.Core.Abstraction;
using JToolbox.Desktop.Core.Services;
using Prism.Ioc;
using System;

namespace Barcodes.Core.Abstraction
{
    public interface IServicesAggregator
    {
        IAppEvents AppEvents { get; }
        IGeneratorService GeneratorService { get; }
        IAppDialogsService AppDialogsService { get; }
        IAppWindowsService AppWindowsService { get; }
        ISystemService SysService { get; }
        IAppSettingsService AppSettingsService { get; }
        IStorageService StorageService { get; }
        IDocExportService DocExportService { get; }
        IStateSaverService StateSaverService { get; }
        IContainerExtension ContainerExtension { get; }
        ILoggerService LoggerService { get; }

        void LogException(string message, Exception exc);

        void OpenAppLocation();

        void OpenStorageLocation(string storagePath);

        void OpenLogs();

        void OpenNewFile(string filePath);
    }
}