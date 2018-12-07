using Barcodes.Core.ViewModels;
using Barcodes.Core.ViewModels.AdditionalInput;
using Barcodes.Core.Views;
using Barcodes.Core.Views.AdditionalInput;
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

        public string OpenNmvsInputWindow()
        {
            var dataContext = containerExtension.Resolve<NmvsInputViewModel>();
            new NmvsInputWindow(dataContext).ShowDialog();
            return dataContext.ResultData;
        }

        public void ShowHelpWindow()
        {
            var dataContext = containerExtension.Resolve<HelpViewModel>();
            ShowModal<HelpWindow>(dataContext);
        }
    }
}
