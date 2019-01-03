using Barcodes.Core;
using Barcodes.Core.Services;
using Barcodes.Core.Services.ViewModelState;
using Barcodes.Core.ViewModels.AdditionalInput;
using Barcodes.Core.Views;
using Barcodes.Services.AppSettings;
using Barcodes.Services.System;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System.Windows;

namespace Barcodes.Startup
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        private GlobalExceptionHandler globalExceptionHandler;

        protected override Window CreateShell()
        {
            return Container.Resolve<ShellWindow>();
        }

        protected override void InitializeShell(Window shell)
        {
            Current.MainWindow = shell;
            Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            var modules = base.CreateModuleCatalog();
            modules.AddModule<CoreModule>();
            return modules;
        }

        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterRequiredTypes(containerRegistry);
            containerRegistry.RegisterSingleton<IAppSettingsService, AppSettingsService>();
            containerRegistry.RegisterSingleton<IAppDialogsService, AppDialogsService>();
            containerRegistry.RegisterSingleton<ISystemService, SystemService>();
            containerRegistry.RegisterSingleton<IViewModelStateSaver, ViewModelStateSaver>();

            RegisterGlobalExceptionHandler();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Container.Resolve<IAppSettingsService>().Load(false);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry) { }

        private void RegisterGlobalExceptionHandler()
        {
            globalExceptionHandler = Container.Resolve<GlobalExceptionHandler>();
            globalExceptionHandler.RegisterEvents(this);
        }
    }
}
