using Barcodes.Core.Services.StateSaver;
using Barcodes.Services.AppSettings;
using Barcodes.Services.DocExport;
using Barcodes.Services.Generator;
using Barcodes.Services.Storage;
using Barcodes.Services.Sys;
using Prism.Events;

namespace Barcodes.Core.Services
{
    public class ServicesContainer : IServicesContainer
    {
        public IEventAggregator EventAggregator { get; }
        public IGeneratorService GeneratorService { get; }
        public IAppDialogsService AppDialogsService { get; }
        public IAppWindowsService AppWindowsService { get; }
        public ISysService SystemService { get; }
        public IAppSettingsService AppSettingsService { get; }
        public IStorageService StorageService { get; }
        public IDocExportService DocExportService { get; }
        public IStateSaverService StateSaverService { get; }

        public ServicesContainer(IEventAggregator eventAggregator, IGeneratorService generatorService, IAppDialogsService appDialogsService,
            IAppWindowsService appWindowsService, ISysService systemService, IAppSettingsService appSettingsService,
            IStorageService storageService, IDocExportService docExportService, IStateSaverService stateSaverService)
        {
            EventAggregator = eventAggregator;
            GeneratorService = generatorService;
            AppDialogsService = appDialogsService;
            AppWindowsService = appWindowsService;
            SystemService = systemService;
            AppSettingsService = appSettingsService;
            StorageService = storageService;
            DocExportService = docExportService;
            StateSaverService = stateSaverService;
        }
    }
}
