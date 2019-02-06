using Barcodes.Core.ViewModels;
using Barcodes.Core.ViewModels.AdditionalInput;
using Barcodes.Core.Views;
using Barcodes.Core.Views.AdditionalInput;
using Barcodes.Services.Generator;
using Barcodes.Services.Windows;
using Prism.Ioc;

namespace Barcodes.Core.Services
{
    public class AppWindowsService : WindowsService, IAppWindowsService
    {
        private readonly IContainerExtension containerExtension;

        public AppWindowsService(IContainerExtension containerExtension)
        {
            this.containerExtension = containerExtension;
        }

        public void OpenBarcodeWindow(object barcodeViewModel)
        {
            Show<BarcodeWindow>(barcodeViewModel);
        }

        public string OpenNmvsProductWindow()
        {
            var dataContext = containerExtension.Resolve<NmvsProductViewModel>();
            var window = new NmvsProductWindow(dataContext);
            window.ShowDialog();
            return dataContext.ResultData;
        }

        public string OpenEan128ProductWindow()
        {
            var dataContext = containerExtension.Resolve<Ean128ProductViewModel>();
            var window = new Ean128ProductWindow(dataContext);
            window.ShowDialog();
            return dataContext.ResultData;
        }

        public void ShowHelpWindow()
        {
            var dataContext = containerExtension.Resolve<HelpViewModel>();
            ShowModal<HelpWindow>(dataContext);
        }

        public GenerationViewModel.GenerationViewModelResult ShowGenerationWindow(BarcodeResultViewModel barcode = null)
        {
            var dataContext = containerExtension.Resolve<GenerationViewModel>();
            dataContext.Load(barcode);
            var window = containerExtension.Resolve<GenerationWindow>();
            ShowModal(window, dataContext);
            return dataContext.Result;
        }
    }
}
