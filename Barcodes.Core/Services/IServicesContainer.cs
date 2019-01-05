using Barcodes.Services.AppSettings;
using Barcodes.Services.DocExport;
using Barcodes.Services.Generator;
using Barcodes.Services.Storage;
using Barcodes.Services.System;
using Prism.Events;

namespace Barcodes.Core.Services
{
    public interface IServicesContainer
    {
        IEventAggregator EventAggregator { get; }
        IBarcodesGeneratorService BarcodesGenerator { get; }
        IAppDialogsService DialogsService { get; }
        IAppWindowsService AppWindowsService { get; }
        ISystemService SystemService { get; }
        IAppSettingsService AppSettingsService { get; }
        IBarcodeStorageService BarcodeStorageService { get; }
        IDocExportService DocExportService { get; }
    }
}
