using Barcodes.Codes;
using Barcodes.Core.Abstraction;
using Barcodes.Core.Models;
using Barcodes.Core.UI.Views;
using Barcodes.Core.ViewModels;
using Barcodes.Core.ViewModels.Templates;
using Barcodes.Extensions;
using Barcodes.Services.AppSettings;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace Barcodes.Core.UI.Services
{
    public class AppWindowsService : IAppWindowsService
    {
        private readonly IContainerExtension containerExtension;
        private readonly WindowManager barcodesWindowsManager = new WindowManager();
        private readonly WindowManager workspacesWindowsManager = new WindowManager();
        private readonly WindowManager quickGeneratorWindowsManager = new WindowManager();

        private readonly Dictionary<Type, Type> templateMapping = new Dictionary<Type, Type>();

        public AppWindowsService(IContainerExtension containerExtension)
        {
            this.containerExtension = containerExtension;
            CreateTemplatesMapping();
        }

        public Window MainWindow => Application.Current.MainWindow;

        public Window ActiveWindow => Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);

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

        public void CloseBarcodesWindows()
        {
            barcodesWindowsManager.CloseAll();
        }

        public void CloseWorkspacesWindows()
        {
            workspacesWindowsManager.CloseAll();
        }

        public void CloseQuickGeneratorsWindows()
        {
            quickGeneratorWindowsManager.CloseAll();
        }

        public void CloseAllWindows()
        {
            barcodesWindowsManager.CloseAll();
            workspacesWindowsManager.CloseAll();
            quickGeneratorWindowsManager.CloseAll();
        }

        public void OpenBarcodeWindow(BarcodeViewModel barcodeViewModel)
        {
            var window = new BarcodeWindow(barcodeViewModel);
            barcodesWindowsManager.RegisterWindow(window, true);
            window.Show();
        }

        public void OpenWorkspaceWindow(WorkspaceViewModel workspaceViewModel)
        {
            var window = new WorkspaceWindow
            {
                DataContext = new WorkspaceWindowViewModel(workspaceViewModel, this)
            };
            workspacesWindowsManager.RegisterWindow(window, true, true);
            window.Show();
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
            window.Owner = ActiveWindow;
            window.ShowDialog();
            return dataContext.ResultData;
        }

        public async Task ShowAboutWindow(Action beforeShow)
        {
            var window = containerExtension.Resolve<AboutWindow>();
            await window.GenerateRandomBarcode();
            beforeShow();
            window.Owner = MainWindow;
            window.ShowDialog();
        }

        public GenerationResult ShowExamplesWindow(ExamplesViewModel examplesViewModel)
        {
            var window = new ExamplesWindow(examplesViewModel)
            {
                Owner = MainWindow
            };
            window.ShowDialog();
            return examplesViewModel.GenerationResult;
        }

        public GenerationResult ShowGenerationWindow(BarcodeViewModel barcode, bool edit, BarcodeTemplate? template = null)
        {
            var dataContext = containerExtension.Resolve<GenerationViewModel>();
            dataContext.Load(barcode, edit, template);
            var window = new GenerationWindow(dataContext)
            {
                Owner = ActiveWindow
            };
            window.ShowDialog();
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
            var window = new InputWindow(dataContext)
            {
                Owner = MainWindow
            };
            window.ShowDialog();
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
            var window = new SelectionWindow(dataContext)
            {
                Owner = MainWindow
            };
            window.ShowDialog();
            return dataContext.Result;
        }

        public SettingsSaveResult ShowSettingsWindow()
        {
            var dataContext = containerExtension.Resolve<SettingsViewModel>();
            var window = new SettingsWindow(dataContext)
            {
                Owner = MainWindow
            };
            window.ShowDialog();
            return dataContext.SettingsSaveResult;
        }

        public AppSettings ShowRawSettingsWindow(AppSettings appSettings)
        {
            var dataContext = containerExtension.Resolve<RawSettingsViewModel>();
            dataContext.LoadSettings(appSettings);
            var window = new RawSettingsWindow(dataContext)
            {
                Owner = ActiveWindow
            };
            window.ShowDialog();
            return dataContext.EditedSettings;
        }

        public void ShowStorageWindow(AppViewModel appViewModel, List<WorkspaceViewModel> workspaces)
        {
            var dataContext = containerExtension.Resolve<StorageViewModel>();
            dataContext.PrepareAndSetWorkspaces(appViewModel, workspaces);
            var window = new StorageWindow
            {
                DataContext = dataContext
            };
            window.Show();
        }

        public void ShowQuickGeneratorWindow()
        {
            var dataContext = containerExtension.Resolve<QuickGeneratorViewModel>();
            var window = new QuickGeneratorWindow(dataContext);
            quickGeneratorWindowsManager.RegisterWindow(window);
            window.Show();
        }

        
    }
}