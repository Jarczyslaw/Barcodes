using Barcodes.Core.Abstraction;
using Barcodes.Core.ViewModels;
using Prism.Ioc;
using System;
using System.Threading.Tasks;

namespace Barcodes.Core.UI.Views
{
    public partial class StartupWindow : BaseWindow
    {
        private readonly IServicesAggregator services;

        public StartupWindow(IServicesAggregator services)
        {
            this.services = services;
            InitializeComponent();
        }

        public Action<ShellWindow> InitilizationCompleted { get; set; }

        protected override async void OnViewShown()
        {
            try
            {
                var delayTask = Task.Delay(1000);
                var initializeTask = Task.Run(() =>
                {
                    // generate dummy barcode to warm up generator's dependencies
                    services.GeneratorService.CreateQRBarcode(50, "Barcodes");

                    // load settings and barcodes
                    var shellViewModel = services.ContainerExtension.Resolve<ShellViewModel>();
                    shellViewModel.App.InitialSequence();
                    return shellViewModel;
                });
                await Task.WhenAll(delayTask, initializeTask);
                var shellWindow = services.ContainerExtension.Resolve<ShellWindow>((typeof(ShellViewModel), initializeTask.Result));
                InitilizationCompleted(shellWindow);
            }
            catch (Exception exc)
            {
                services.LogException("Error in app loading", exc);
            }
            finally
            {
                Close();
            }
        }
    }
}