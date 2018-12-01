using Barcodes.Core.ViewModels;
using Barcodes.Core.Views;
using Barcodes.Services.Dialogs;
using Barcodes.Services.Windows;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Core.Services
{
    public class BarcodeWindowsService : WindowsService, IBarcodeWindowsService
    {
        private readonly IContainerExtension containerExtension;

        public BarcodeWindowsService(IContainerExtension containerExtension)
        {
            this.containerExtension = containerExtension;
        }

        public void OpenBarcodeWindow(object barcodeViewModel)
        {
            Show<BarcodeWindow>(barcodeViewModel);
        }

        public string OpenNmvsInputWindow()
        {
            var viewModel = containerExtension.Resolve<NmvsInputViewModel>();
            new NmvsInputWindow(viewModel).ShowDialog();
            return viewModel.ResultData;
        }
    }
}
