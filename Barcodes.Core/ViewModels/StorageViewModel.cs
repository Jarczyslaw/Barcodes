using Barcodes.Core.Common;
using Barcodes.Services.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Barcodes.Core.ViewModels
{
    public class StorageViewModel : BindableBase, ICloseSource
    {
        private ObservableCollection<WorkspaceViewModel> workspaces;
        private IDialogsService dialogsService;

        public StorageViewModel(IDialogsService dialogsService)
        {
            this.dialogsService = dialogsService;
        }

        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get => workspaces;
            set => SetProperty(ref workspaces, value);
        }

        public List<WorkspaceViewModel> WorkspacesToImport { get; private set; }

        public Action OnClose { get; set; }

        public DelegateCommand CheckAllCommand => new DelegateCommand(() => SetWorkspacesChecked(Workspaces, true));

        public DelegateCommand UncheckAllCommand => new DelegateCommand(() => SetWorkspacesChecked(Workspaces, false));

        public DelegateCommand<WorkspaceViewModel> WorkspaceCheckAllCommand => new DelegateCommand<WorkspaceViewModel>(w => SetBarcodesChecked(w.Barcodes, true));

        public DelegateCommand<WorkspaceViewModel> WorkspaceUncheckAllCommand => new DelegateCommand<WorkspaceViewModel>(w => SetBarcodesChecked(w.Barcodes, false));

        public DelegateCommand CloseCommand => new DelegateCommand(() => OnClose?.Invoke());

        public DelegateCommand AcceptCommand => new DelegateCommand(() =>
        {
            
        });

        public void SetWorkspaces(List<WorkspaceViewModel> workspaces)
        {
            Workspaces = new ObservableCollection<WorkspaceViewModel>();
            Workspaces.AddRange(workspaces);
            SetWorkspacesChecked(Workspaces, true);
        }

        private void SetWorkspacesChecked(ICollection<WorkspaceViewModel> workspaces, bool isChecked)
        {
            foreach (var workspace in workspaces)
            {
                SetBarcodesChecked(workspace.Barcodes, isChecked);
            }
        }

        private void SetBarcodesChecked(ICollection<BarcodeViewModel> barcodes, bool isChecked)
        {
            foreach (var barcode in barcodes)
            {
                barcode.IsChecked = isChecked;
            }
        }

        private List<WorkspaceViewModel> GetCheckedWorkspaces()
        {
            var result = new List<WorkspaceViewModel>();

            return result;
        }
    }
}