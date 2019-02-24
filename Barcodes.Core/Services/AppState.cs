using Barcodes.Core.ViewModels;
using Barcodes.Extensions;
using Barcodes.Services.Generator;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;

namespace Barcodes.Core.Services
{
    public class AppState : BindableBase, IAppState
    {
        private string statusMessage = string.Empty;
        private WorkspaceViewModel selectedWorkspace;
        private ObservableCollection<WorkspaceViewModel> workspaces;
        private string busyMessage = string.Empty;

        private readonly IGeneratorService generatorService;

        public AppState(IGeneratorService generatorService)
        {
            this.generatorService = generatorService;

            Workspaces = new ObservableCollection<WorkspaceViewModel>();
        }

        public bool IsBusy { get; set; }

        public bool IsEmpty
        {
            get => workspaces.Count == 0;
        }

        public string BusyMessage
        {
            get => busyMessage;
            set
            {
                SetProperty(ref busyMessage, value);
                IsBusy = !string.IsNullOrEmpty(busyMessage);
                RaisePropertyChanged(nameof(IsBusy));
            }
        }

        public string StatusMessage
        {
            get => statusMessage;
            set => SetProperty(ref statusMessage, value);
        }

        public int BarcodesCount
        {
            get => Workspaces.Sum(w => w.Barcodes.Count);
        }

        public WorkspaceViewModel SelectedWorkspace
        {
            get => selectedWorkspace;
            set => SetProperty(ref selectedWorkspace, value);
        }

        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get => workspaces;
            set => SetProperty(ref workspaces, value);
        }

        public void AddWorkspace(WorkspaceViewModel workspace)
        {
            workspace.OnMessageUpdate += message => StatusMessage = message;
            workspace.OnCounterUpdate += () => RaisePropertyChanged(nameof(BarcodesCount));

            Workspaces.Add(workspace);
            SelectedWorkspace = workspace;
        }

        public void SetMessageAndCounter(string message)
        {
            StatusMessage = message;
            RaisePropertyChanged(nameof(BarcodesCount));
        }
    }
}
