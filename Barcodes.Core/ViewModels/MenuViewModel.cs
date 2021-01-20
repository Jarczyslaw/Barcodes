using Prism.Commands;
using Prism.Mvvm;
using System;

namespace Barcodes.Core.ViewModels
{
    public class MenuViewModel : BindableBase
    {
        public MenuViewModel(AppViewModel app)
        {
            SaveCommand = new DelegateCommand(() => app.Save(false, false));
            SaveAsCommand = new DelegateCommand(() => app.Save(true, false));
            LoadFromFileCommand = new DelegateCommand(app.LoadStorageFromFile);
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
            ImportStorageCommand = new DelegateCommand(app.ImportStorage);
            ShowSettingsCommand = new DelegateCommand(app.ShowSettings);
            CloseAllWindowsCommand = new DelegateCommand(app.CloseAllWindows);
            ClearCommand = new DelegateCommand(app.Clear);
            CreateNewStorageCommand = new DelegateCommand(app.CreateNewStorage);
            PrintCommand = new DelegateCommand(app.Print);
            QuickGeneratorCommand = new DelegateCommand(app.ShowQuickGenerator);
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
        public DelegateCommand ImportStorageCommand { get; }
        public DelegateCommand ShowSettingsCommand { get; }
        public DelegateCommand CloseAllWindowsCommand { get; }
        public DelegateCommand ClearCommand { get; }
        public DelegateCommand CreateNewStorageCommand { get; }
        public DelegateCommand PrintCommand { get; }
        public DelegateCommand QuickGeneratorCommand { get; }

        public Action OnClose { get; set; }
    }
}