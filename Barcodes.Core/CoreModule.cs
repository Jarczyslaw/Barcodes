using Barcodes.Core.Services;
using Barcodes.Core.Services.ViewModelState;
using Barcodes.Core.Views;
using Barcodes.Services.DocExport;
using Barcodes.Services.Generator;
using Barcodes.Services.Storage;
using Barcodes.Services.Windows;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Barcodes.Core
{
    public class CoreModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionNames.Core, typeof(ShellView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IBarcodesGeneratorService, BarcodesGeneratorService>();
            containerRegistry.RegisterSingleton<IWindowsService, WindowsService>();
            containerRegistry.RegisterSingleton<IAppWindowsService, AppWindowsService>();
            containerRegistry.RegisterSingleton<IBarcodeStorageService, BarcodeStorageService>();
            containerRegistry.RegisterSingleton<IDocExportService, DocExportService>();
            containerRegistry.RegisterSingleton<IServicesContainer, ServicesContainer>();
            containerRegistry.RegisterSingleton<IViewModelStateSaver, ViewModelStateSaver>();
        }
    }
}
