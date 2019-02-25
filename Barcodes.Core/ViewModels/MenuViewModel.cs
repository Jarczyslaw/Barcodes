using Prism.Commands;
using Prism.Mvvm;
using System;

namespace Barcodes.Core.ViewModels
{
    public class MenuViewModel : BindableBase
    {
        public MenuViewModel(AppViewModel app) 
        {
            SaveToFileCommand = new DelegateCommand(app.SaveToFile);
            LoadFromFileCommand = new DelegateCommand(app.LoadBarcodesFromFile);
            OpenStorageLocationCommand = new DelegateCommand(app.OpenStorageLocation);
            CloseCommand = new DelegateCommand(() => OnClose?.Invoke());
            ExportToPdfCommand = new DelegateCommand(app.ExportToPdf);
            ShowHelpCommand = new DelegateCommand(app.ShowHelp);
            AddNewWorkspaceCommand = new DelegateCommand(app.AddNewWorkspace);
            AddNewBarcodeCommand = new DelegateCommand(app.AddNewBarcode);
        }

        public DelegateCommand SaveToFileCommand { get; }
        public DelegateCommand LoadFromFileCommand { get; }
        public DelegateCommand OpenStorageLocationCommand { get; }
        public DelegateCommand CloseCommand { get; }
        public DelegateCommand ExportToPdfCommand { get; }
        public DelegateCommand ShowHelpCommand { get; }
        public DelegateCommand AddNewWorkspaceCommand { get; }
        public DelegateCommand AddNewBarcodeCommand { get; }

        public Action OnClose { get; set; }
    }
}
