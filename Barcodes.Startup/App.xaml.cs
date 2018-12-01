using Prism.Unity;
using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Barcodes.Core;
using Barcodes.Services.Generator;
using Barcodes.Services.Storage;
using Barcodes.Services.Dialogs;
using Barcodes.Services.Windows;
using Barcodes.Core.Views;

namespace Barcodes.Startup
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        private IContainerExtension container;
        private GlobalExceptionHandler globalExceptionHandler;

        protected override Window CreateShell()
        {
            return Container.Resolve<ShellWindow>();
        }

        protected override IContainerExtension CreateContainerExtension()
        {
            container = base.CreateContainerExtension();
            return container;
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
            containerRegistry.RegisterSingleton<IDialogsService, DialogsService>();
            RegisterGlobalExceptionHandler();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry) { }

        private void RegisterGlobalExceptionHandler()
        {
            globalExceptionHandler = container.Resolve<GlobalExceptionHandler>();
            globalExceptionHandler.RegisterEvents(this);
        }
    }
}
