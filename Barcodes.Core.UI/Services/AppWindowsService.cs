using Barcodes.Core.Abstraction;
using Barcodes.Core.UI.Views;
using Barcodes.Core.UI.Views.AdditionalInput;
using Barcodes.Core.ViewModels;
using Barcodes.Core.ViewModels.AdditionalInput;
using Barcodes.Core.ViewModelsInput;
using Barcodes.Core.ViewModelsResult;
using Barcodes.Services.Windows;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Barcodes.Core.UI.Services
{
    public class AppWindowsService : WindowsService, IAppWindowsService
    {
        private readonly IContainerExtension containerExtension;
        private readonly WindowManager barcodesWindowsManager = new WindowManager();
        private readonly WindowManager workspacesWindowsManager = new WindowManager();

        public AppWindowsService(IContainerExtension containerExtension)
        {
            this.containerExtension = containerExtension;
        }

        public int WindowsCount => barcodesWindowsManager.WindowsCount + workspacesWindowsManager.WindowsCount;

        public void CloseBarcodesAndWorkspacesWindows()
        {
            barcodesWindowsManager.CloseAll();
            workspacesWindowsManager.CloseAll();
        }

        public void OpenBarcodeWindow(BarcodeViewModel barcodeViewModel)
        {
            var window = new BarcodeWindow(barcodeViewModel);
            barcodesWindowsManager.RegisterWindow(window);
            Show(window);
        }

        public void OpenWorkspaceWindow(WorkspaceViewModel workspaceViewModel)
        {
            var dataContext = new WorkspaceWindowViewModel(workspaceViewModel, this);
            var window = new WorkspaceWindow();
            workspacesWindowsManager.RegisterWindow(window, true);
            Show(window, dataContext);
        }

        public string OpenNmvsProductWindow(string nmvsData)
        {
            var dataContext = containerExtension.Resolve<NmvsProductViewModel>();
            dataContext.LoadData(nmvsData);
            var window = new NmvsProductWindow(dataContext);
            ShowDialog(window);
            return dataContext.ResultData;
        }

        public string OpenEan128ProductWindow(string ean128Data)
        {
            var dataContext = containerExtension.Resolve<Ean128ProductViewModel>();
            dataContext.LoadData(ean128Data);
            var window = new Ean128ProductWindow(dataContext);
            ShowDialog(window);
            return dataContext.ResultData;
        }

        public void ShowAboutWindow()
        {
            var window = containerExtension.Resolve<AboutWindow>();
            ShowDialog(window);
        }

        public BarcodeViewModel ShowExamplesWindow()
        {
            var dataContext = containerExtension.Resolve<ExamplesViewModel>();
            var window = new ExamplesWindow(dataContext);
            ShowDialog(window, dataContext);
            return dataContext.ResultBarcode;
        }

        public GenerationViewModelResult ShowGenerationWindow(BarcodeViewModel barcode, bool edit)
        {
            var dataContext = containerExtension.Resolve<GenerationViewModel>();
            dataContext.Load(barcode, edit);
            var window = new GenerationWindow(dataContext);
            ShowDialog(window);
            return dataContext.Result;
        }

        public string ShowWorkspaceNameWindow(string currentName, Func<string, bool> validationRule)
        {
            if (string.IsNullOrEmpty(currentName))
            {
                currentName = "Default workspace";
            }

            var input = new InputViewModelInput
            {
                Title = "Barcodes - Workspace",
                ContentHeader = "Enter workspace's name",
                Label = "Name:",
                InputValue = currentName,
                ValidationRule = validationRule
            };

            var dataContext = new InputViewModel(input);
            var window = new InputWindow(dataContext);
            ShowDialog(window);
            return dataContext.Result;
        }

        public WorkspaceViewModel SelectBarcodesWorkspace(IEnumerable<WorkspaceViewModel> workspaces)
        {
            var selectedWorkspace = workspaces.First();
            var input = new SelectionViewModelInput<WorkspaceViewModel>
            {
                Title = "Barcodes - Workspaces",
                ContentHeader = "Select desired workspace",
                Label = "Workspace:",
                Items = workspaces,
                SelectedItem = selectedWorkspace,
                DisplayMemberPath = "Name"
            };
            var dataContext = new SelectionViewModel<WorkspaceViewModel>(input);
            var window = new SelectionWindow(dataContext);
            ShowDialog(window);
            return dataContext.Result;
        }

        public bool ShowSettingsWindow()
        {
            var dataContext = containerExtension.Resolve<SettingsViewModel>();
            ShowDialog(new SettingsWindow(dataContext));
            return dataContext.SettingsSaved;
        }
    }
}