using Barcodes.Core.Abstraction;
using Prism.Commands;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Barcodes.Core.ViewModels
{
    public class StorageViewModel : BaseViewModel
    {
        private readonly IAppDialogsService appDialogsService;

        private WorkspaceViewModel selectedWorkspace;
        private ObservableCollection<WorkspaceViewModel> workspaces;
        private AppViewModel appViewModel;

        public StorageViewModel(IAppDialogsService appDialogsService)
        {
            this.appDialogsService = appDialogsService;
        }

        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get => workspaces;
            set => SetProperty(ref workspaces, value);
        }

        public WorkspaceViewModel SelectedWorkspace
        {
            get => selectedWorkspace;
            set => SetProperty(ref selectedWorkspace, value);
        }

        public List<BarcodeViewModel> SelectedBarcodes { get; set; }

        public DelegateCommand ImportBarcodesCommand => new DelegateCommand(ImportBarcodes);

        public DelegateCommand ImportWorkspaceCommand => new DelegateCommand(ImportWorkspace);

        public DelegateCommand ImportAllCommand => new DelegateCommand(() => ImportWorkspaces(Workspaces));

        public void PrepareAndSetWorkspaces(AppViewModel appViewModel, List<WorkspaceViewModel> workspaces)
        {
            this.appViewModel = appViewModel;
            Workspaces = new ObservableCollection<WorkspaceViewModel>();
            Workspaces.AddRange(workspaces);
        }

        private void ImportBarcodes()
        {
            if (SelectedBarcodes == null)
            {
                appDialogsService.ShowError("No barcodes selected");
                return;
            }

            var toImport = SelectedBarcodes.OrderBy(b => SelectedWorkspace.Barcodes.IndexOf(b))
                    .Select(b => new BarcodeViewModel(b))
                    .ToList();
            appViewModel.ImportBarcodes(toImport);
        }

        private void ImportWorkspace()
        {
            if (SelectedWorkspace == null)
            {
                appDialogsService.ShowError("No workspace selected");
                return;
            }

            ImportWorkspaces(new List<WorkspaceViewModel> { SelectedWorkspace });
        }

        private void ImportWorkspaces(IEnumerable<WorkspaceViewModel> workspaces)
        {
            var toImport = workspaces.Select(w => new WorkspaceViewModel(w))
                .ToList();
            appViewModel.ImportWorkspaces(toImport);
        }
    }
}