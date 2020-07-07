﻿using Barcodes.Core.Common;
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

        public DelegateCommand CheckAllCommand => new DelegateCommand(() => SetChecked(Workspaces, true));

        public DelegateCommand UncheckAllCommand => new DelegateCommand(() => SetChecked(Workspaces, false));

        public DelegateCommand CloseCommand => new DelegateCommand(() => OnClose?.Invoke());

        public void SetWorkspaces(List<WorkspaceViewModel> workspaces)
        {
            Workspaces = new ObservableCollection<WorkspaceViewModel>();
            Workspaces.AddRange(workspaces);
            SetChecked(Workspaces, true);
        }

        private void SetChecked(ICollection<WorkspaceViewModel> workspaces, bool isChecked)
        {
            foreach (var workspace in workspaces)
            {
                foreach (var barcode in workspace.Barcodes)
                {
                    barcode.IsChecked = isChecked;
                }
            }
        }
    }
}