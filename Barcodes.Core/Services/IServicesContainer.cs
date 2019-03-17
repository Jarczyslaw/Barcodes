using Barcodes.Core.Services.StateSaver;
using Barcodes.Services.AppSettings;
using Barcodes.Services.DocExport;
using Barcodes.Services.Generator;
using Barcodes.Services.Storage;
using Barcodes.Services.Sys;
using Prism.Events;

namespace Barcodes.Core.Services
{
    public interface IServicesContainer
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
    }
}
