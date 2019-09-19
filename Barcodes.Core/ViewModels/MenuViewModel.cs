using Prism.Commands;
using Prism.Mvvm;
using System;

namespace Barcodes.Core.ViewModels
{
    public class MenuViewModel : BindableBase
    {
        public MenuViewModel(AppViewModel app)
        {
            SaveCommand = new DelegateCommand(() => app.Save(true, false));
            SaveAsCommand = new DelegateCommand(() => app.Save(true, true));
            LoadFromFileCommand = new DelegateCommand(app.LoadBarcodesFromFile);
            OpenAppLocationCommand = new DelegateCommand(app.OpenAppLocation);
            OpenStorageLocationCommand = new DelegateCommand(app.OpenStorageLocation);
            CloseCommand = new DelegateCommand(() => OnClose?.Invoke());
            ExportToPdfCommand = new DelegateCommand(app.ExportToPdf);
            ShowAboutCommand = new DelegateCommand(app.ShowAbout);
            ShowExamplesCommand = new DelegateCommand(app.ShowExamples);
            AddNewWorkspaceCommand = new DelegateCommand(app.AddNewWorkspace);
            AddNewBarcodeCommand = new DelegateCommand(() => app.AddNewBarcode(null, false));
            ImportBarcodesCommand = new DelegateCommand(app.ImportBarcodes);
            ImportWorkspacesCommand = new DelegateCommand(app.ImportWorkspaces);
            ShowSettingsCommand = new DelegateCommand(app.ShowSettings);
        }

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand SaveAsCommand { get; }
        public DelegateCommand LoadFromFileCommand { get; }
        public DelegateCommand OpenAppLocationCommand { get; }
        public DelegateCommand OpenStorageLocationCommand { get; }
        public DelegateCommand CloseCommand { get; }
        public DelegateCommand ExportToPdfCommand { get; }
        public DelegateCommand ShowAboutCommand { get; }
        public DelegateCommand ShowExamplesCommand { get; }
        public DelegateCommand AddNewWorkspaceCommand { get; }
        public DelegateCommand AddNewBarcodeCommand { get; }
        public DelegateCommand ImportBarcodesCommand { get; }
        public DelegateCommand ImportWorkspacesCommand { get; }
        public DelegateCommand ShowSettingsCommand { get; }

        public Action OnClose { get; set; }
    }
}