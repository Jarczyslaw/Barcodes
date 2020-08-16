using Barcodes.Core.Common;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Barcodes.Core.ViewModels
{
    public class StorageViewModel : BindableBase, ICloseSource
    {
        private WorkspaceViewModel selectedWorkspace;
        private ObservableCollection<WorkspaceViewModel> workspaces;
        private AppViewModel appViewModel;

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

        public Action OnClose { get; set; }

        public DelegateCommand<WorkspaceViewModel> ImportWorkspaceCommand => new DelegateCommand<WorkspaceViewModel>(workspace =>
        {
            ImportWorkspaces(new List<WorkspaceViewModel>
            {
                workspace
            });
        });

        public DelegateCommand ImportAllCommand => new DelegateCommand(() => ImportWorkspaces(Workspaces));

        public DelegateCommand ImportBarcodesCommand => new DelegateCommand(ImportBarcodes);

        public DelegateCommand CloseCommand => new DelegateCommand(() => OnClose?.Invoke());

        public void PrepareAndSetWorkspaces(AppViewModel appViewModel, List<WorkspaceViewModel> workspaces)
        {
            this.appViewModel = appViewModel;
            Workspaces = new ObservableCollection<WorkspaceViewModel>();
            Workspaces.AddRange(workspaces);
            foreach (var workspace in Workspaces)
            {
                workspace.IsChecked = false;
                workspace.DefaultWorkspace = false;
            }
        }

        private void ImportBarcodes()
        {
            if (SelectedBarcodes != null)
            {
                var toImport = SelectedBarcodes.OrderBy(b => SelectedWorkspace.Barcodes.IndexOf(b))
                    .Select(b => new BarcodeViewModel(b))
                    .ToList();
                appViewModel.ImportBarcodes(toImport);
            }
        }

        private void ImportWorkspaces(IEnumerable<WorkspaceViewModel> workspaces)
        {
            var toImport = workspaces.Select(w => new WorkspaceViewModel(w))
                .ToList();
            appViewModel.ImportWorkspaces(toImport);
        }
    }
}