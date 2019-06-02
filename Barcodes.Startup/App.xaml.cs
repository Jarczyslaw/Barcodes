using Barcodes.Core.Services;
using Barcodes.Core.Services.StateSaver;
using Barcodes.Core.UI.Views;
using Barcodes.Services.AppSettings;
using Barcodes.Services.Dialogs;
using Barcodes.Services.DocExport;
using Barcodes.Services.Generator;
using Barcodes.Services.Storage;
using Barcodes.Services.Sys;
using Barcodes.Services.Windows;
using Barcodes.SingleInstance;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

namespace Barcodes.Startup
{
    public partial class App : PrismApplication
    {
        private AppGlobalExceptionHandler globalExceptionHandler;
        private SingleInstanceManager singleInstanceManager;
        private readonly string appKey = "d78ce89b-fec3-4fb7-9f9e-43e16791c130";

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                singleInstanceManager = new SingleInstanceManager(appKey, TimeSpan.Zero);
                if (!AppConfig.MultiInst && !singleInstanceManager.FirstInstance)
                {
                    MessageBox.Show("App is currently running", "Barcodes", MessageBoxButton.OK, MessageBoxImage.Stop);
                    singleInstanceManager.SendNofitication();
                    Shutdown();
                }
                else
                {
                    TrySetCulture();
                    base.OnStartup(e);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show($"Exception on startup occured: {exc.Message}", "Barcodes", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<ShellWindow>();
        }

        protected override void InitializeShell(Window shell)
        {
            Current.MainWindow = shell;
            Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        protected override void RegisterRequiredTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterRequiredTypes(containerRegistry);

            containerRegistry.RegisterInstance(singleInstanceManager);
            containerRegistry.RegisterSingleton<IAppSettingsService, AppSettingsService>();
            var appDialogsService = new AppDialogsService();
            containerRegistry.RegisterInstance<IDialogsService>(appDialogsService);
            containerRegistry.RegisterInstance<IAppDialogsService>(appDialogsService);
            containerRegistry.RegisterSingleton<ISysService, SysService>();
            containerRegistry.RegisterSingleton<IGeneratorService, GeneratorService>();
            containerRegistry.RegisterSingleton<IWindowsService, WindowsService>();
            containerRegistry.RegisterSingleton<IAppWindowsService, AppWindowsService>();
            containerRegistry.RegisterSingleton<IStorageService, StorageService>();
            containerRegistry.RegisterSingleton<IDocExportService, DocExportService>();
            containerRegistry.RegisterSingleton<IServicesContainer, ServicesContainer>();
            containerRegistry.RegisterSingleton<IStateSaverService, StateSaverService>();
            RegisterGlobalExceptionHandler();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Container.Resolve<IAppSettingsService>().Load(false);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(viewType =>
            {
                var viewModelResolver = new ViewModelResolver();
                return viewModelResolver.Resolve(viewType);
            });
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            singleInstanceManager.Release();
        }

        private void RegisterGlobalExceptionHandler()
        {
            globalExceptionHandler = Container.Resolve<AppGlobalExceptionHandler>();
            globalExceptionHandler.Register();
        }

        private void TrySetCulture()
        {
            try
            {
                var cultureInfo = new CultureInfo("en-EN");
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
                    new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            }
            catch { }
        }
    }
}