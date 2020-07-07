using Barcodes.Codes;
using Barcodes.Core.Abstraction;
using Barcodes.Core.Models;
using Barcodes.Core.UI.Views;
using Barcodes.Core.ViewModels;
using Barcodes.Core.ViewModels.Templates;
using Barcodes.Extensions;
using Barcodes.Services.AppSettings;
using Barcodes.Services.Windows;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Barcodes.Core.UI.Services
{
    public class AppWindowsService : WindowsService, IAppWindowsService
    {
        private readonly IContainerExtension containerExtension;
        private readonly WindowManager barcodesWindowsManager = new WindowManager();
        private readonly WindowManager workspacesWindowsManager = new WindowManager();

        private readonly Dictionary<Type, Type> templateMapping = new Dictionary<Type, Type>();

        public AppWindowsService(IContainerExtension containerExtension)
        {
            this.containerExtension = containerExtension;
            CreateTemplatesMapping();
        }

        public int WindowsCount => barcodesWindowsManager.WindowsCount + workspacesWindowsManager.WindowsCount;

        private void CreateTemplatesMapping()
        {
            var windows = GetAllWindows();
            foreach (var viewModel in GetViewModels())
            {
                var mapping = windows.Find(w => w.Name == viewModel.Name.Replace("ViewModel", "Window"));
                if (mapping != null)
                {
                    templateMapping.Add(viewModel, mapping);
                }
            }
        }

        private List<Type> GetViewModels()
        {
            return Assembly.GetAssembly(typeof(BaseTemplateViewModel)).GetDerivedTypes(typeof(BaseTemplateViewModel));
        }

        private List<Type> GetAllWindows()
        {
            return Assembly.GetExecutingAssembly().GetDerivedTypes(typeof(Window));
        }

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

        public TemplateResult OpenTemplateWindow<TViewModel>(string data)
        {
            var viewModelType = typeof(TViewModel);
            return OpenTemplateWindow(viewModelType, templateMapping[viewModelType], data);
        }

        private TemplateResult OpenTemplateWindow(Type viewModelType, Type viewType, string data)
        {
            var method = typeof(AppWindowsService).GetMethod(nameof(GenericOpenTemplateWindow), BindingFlags.NonPublic | BindingFlags.Instance);
            var generic = method.MakeGenericMethod(viewModelType, viewType);
            return (TemplateResult)generic.Invoke(this, new object[] { data });
        }

        private TemplateResult GenericOpenTemplateWindow<TViewModel, TWindow>(string data)
            where TViewModel : BaseTemplateViewModel
            where TWindow : Window
        {
            var dataContext = containerExtension.Resolve<TViewModel>();
            dataContext.LoadData(data);
            var window = containerExtension.Resolve<TWindow>((typeof(object), dataContext));
            ShowDialog(window);
            return dataContext.ResultData;
        }

        public void ShowAboutWindow()
        {
            var window = containerExtension.Resolve<AboutWindow>();
            ShowDialog(window);
        }

        public ExampleBarcodeViewModel ShowExamplesWindow()
        {
            var dataContext = containerExtension.Resolve<ExamplesViewModel>();
            var window = new ExamplesWindow(dataContext);
            ShowDialog(window, dataContext);
            return dataContext.ResultBarcode;
        }

        public GenerationResult ShowGenerationWindow(BarcodeViewModel barcode, bool edit, BarcodeTemplate? template = null)
        {
            var dataContext = containerExtension.Resolve<GenerationViewModel>();
            dataContext.Load(barcode, edit, template);
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

            var input = new ValueInput
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
            var input = new SelectionInput<WorkspaceViewModel>
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

        public SettingsSaveResult ShowSettingsWindow()
        {
            var dataContext = containerExtension.Resolve<SettingsViewModel>();
            ShowDialog(new SettingsWindow(dataContext));
            return dataContext.SettingsSaveResult;
        }

        public AppSettings ShowRawSettingsWindow(AppSettings appSettings)
        {
            var dataContext = containerExtension.Resolve<RawSettingsViewModel>();
            dataContext.LoadSettings(appSettings);
            ShowDialog(new RawSettingsWindow(dataContext));
            return dataContext.EditedSettings;
        }

        public List<WorkspaceViewModel> ShowStorageWindow(List<WorkspaceViewModel> workspaces)
        {
            var dataContext = containerExtension.Resolve<StorageViewModel>();
            dataContext.SetWorkspaces(workspaces);
            ShowDialog(new StorageWindow(dataContext));
            return dataContext.WorkspacesToImport;
        }
    }
}