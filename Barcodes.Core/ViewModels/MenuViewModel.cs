using Prism.Commands;
using Prism.Mvvm;
using System;

namespace Barcodes.Core.ViewModels
{
    public class MenuViewModel : BindableBase
    {
        public MenuViewModel(AppViewModel appViewModel)
        {
            SaveToFileCommand = new DelegateCommand(appViewModel.SaveBarcodesToFile);
            LoadFromFileCommand = new DelegateCommand(appViewModel.LoadBarcodesFromFile);
            OpenStorageLocationCommand = new DelegateCommand(appViewModel.OpenStorageLocation);
            CloseCommand = new DelegateCommand(() => OnClose?.Invoke());
            ExportToPdfCommand = new DelegateCommand(appViewModel.ExportToPdf);
            ShowHelpCommand = new DelegateCommand(appViewModel.ShowHelp);
            AddNewWorkspaceCommand = new DelegateCommand(appViewModel.AddNewWorkspace);
        }

        public DelegateCommand SaveToFileCommand { get; }
        public DelegateCommand LoadFromFileCommand { get; }
        public DelegateCommand OpenStorageLocationCommand { get; }
        public DelegateCommand CloseCommand { get; }
        public DelegateCommand ExportToPdfCommand { get; }
        public DelegateCommand ShowHelpCommand { get; }
        public DelegateCommand AddNewWorkspaceCommand { get; }

        public Action OnClose { get; set; }
    }
}
