using Barcodes.Core.UI.Views;
using Barcodes.Core.UI.Views.AdditionalInput;
using Barcodes.Core.ViewModels;
using Barcodes.Core.ViewModels.AdditionalInput;
using Barcodes.Services.Windows;
using Prism.Ioc;
using System;

namespace Barcodes.Core.Services
{
    public class AppWindowsService : WindowsService, IAppWindowsService
    {
        private readonly IContainerExtension containerExtension;
        private readonly IAppDialogsService appDialogsService;

        public AppWindowsService(IContainerExtension containerExtension, IAppDialogsService appDialogsService)
        {
            this.containerExtension = containerExtension;
            this.appDialogsService = appDialogsService;
        }

        public void OpenBarcodeWindow(object barcodeViewModel)
        {
            Show<BarcodeWindow>(barcodeViewModel, GetActiveWindow());
        }

        public string OpenNmvsProductWindow(string nmvsData)
        {
            var dataContext = containerExtension.Resolve<NmvsProductViewModel>();
            dataContext.LoadData(nmvsData);
            Show<NmvsProductWindow>(dataContext, GetActiveWindow(), true);
            return dataContext.ResultData;
        }

        public string OpenEan128ProductWindow(string ean128Data)
        {
            var dataContext = containerExtension.Resolve<Ean128ProductViewModel>();
            dataContext.LoadData(ean128Data);
            Show<Ean128ProductWindow>(dataContext, GetActiveWindow(), true);
            return dataContext.ResultData;
        }

        public void ShowHelpWindow()
        {
            var window = containerExtension.Resolve<HelpWindow>();
            Show(window, null, GetActiveWindow(), true);
        }

        public GenerationViewModelResult ShowGenerationWindow(BarcodeResultViewModel barcode = null)
        {
            var dataContext = containerExtension.Resolve<GenerationViewModel>();
            dataContext.Load(barcode);
            Show<GenerationWindow>(dataContext, GetActiveWindow(), true);
            return dataContext.Result;
        }

        public string ShowWorkspaceNameWindow(string currentName = "Default workspace")
        {
            return ShowInputWindow("Barcodes - Workspace name", "Enter workspace's name", "Name:", currentName, (value) =>
            {
                if (string.IsNullOrEmpty(value))
                {
                    appDialogsService.ShowError("Invalid workspace's name");
                    return false;
                }

                return true;
            });
        }

        private string ShowInputWindow(string title, string contentHeader, string label, string inputValue, Func<string, bool> validationRule)
        {
            var dataContext = new InputViewModel(title, contentHeader, label, inputValue, validationRule);
            Show<InputWindow>(dataContext, GetActiveWindow(), true);
            return dataContext.Result;
        }
    }
}
