using Barcodes.Core.Common;
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
        private AppViewModel appViewModel;

        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get => workspaces;
            set => SetProperty(ref workspaces, value);
        }

        public List<BarcodeViewModel> SelectedBarcodes { get; set; }

        public Action OnClose { get; set; }

        public DelegateCommand<WorkspaceViewModel> ImportWorkspaceCommand => new DelegateCommand<WorkspaceViewModel>(w =>
        {
        });

        public DelegateCommand ImportBarcodesCommand => new DelegateCommand(() =>
        {
        });

        public DelegateCommand ImportAllCommand => new DelegateCommand(() =>
        {
        });

        public DelegateCommand CloseCommand => new DelegateCommand(() => OnClose?.Invoke());

        public void SetWorkspaces(AppViewModel appViewModel, List<WorkspaceViewModel> workspaces)
        {
            this.appViewModel = appViewModel;
            Workspaces = new ObservableCollection<WorkspaceViewModel>();
            Workspaces.AddRange(workspaces);
        }
    }
}