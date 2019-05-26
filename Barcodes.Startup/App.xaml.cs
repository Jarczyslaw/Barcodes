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
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

namespace Barcodes.Startup
{
    public partial class App : PrismApplication
    {
        private AppGlobalExceptionHandler globalExceptionHandler;

        protected override void OnStartup(StartupEventArgs e)
        {
            if (!AppConfig.MultiInst && !SingleInstanceCheck.Check())
            {
                MessageBox.Show("App is currently running", "Barcodes", MessageBoxButton.OK, MessageBoxImage.Stop);
                Shutdown();
            }
            else
            {
                TrySetCulture();
                base.OnStartup(e);
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