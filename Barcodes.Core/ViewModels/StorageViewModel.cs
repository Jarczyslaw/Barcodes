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
            appViewModel.ImportWorkspaces(new List<WorkspaceViewModel>
            {
                workspace
            });
        });

        public DelegateCommand ImportBarcodesCommand => new DelegateCommand(() =>
        {
            if (SelectedBarcodes != null)
            {
                appViewModel.ImportBarcodes(SelectedBarcodes);
            }
        });

        public DelegateCommand ImportAllCommand => new DelegateCommand(() => appViewModel.ImportWorkspaces(Workspaces.ToList()));

        public DelegateCommand CloseCommand => new DelegateCommand(() => OnClose?.Invoke());

        public void SetWorkspaces(AppViewModel appViewModel, List<WorkspaceViewModel> workspaces)
        {
            this.appViewModel = appViewModel;
            Workspaces = new ObservableCollection<WorkspaceViewModel>();
            Workspaces.AddRange(workspaces);
        }
    }
}