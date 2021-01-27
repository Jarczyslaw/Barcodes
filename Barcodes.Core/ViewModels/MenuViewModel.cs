using Barcodes.Core.Abstraction;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.IO;

namespace Barcodes.Core.ViewModels
{
    public class MenuViewModel : BindableBase
    {
        private readonly IServicesAggregator services;
        private readonly AppViewModel app;

        public MenuViewModel(AppViewModel app, IServicesAggregator services)
        {
            this.services = services;
            this.app = app;
        }

        public DelegateCommand SaveCommand => new DelegateCommand(() => app.Save(false, false));
        public DelegateCommand SaveAsCommand => new DelegateCommand(() => app.Save(true, false));
        public DelegateCommand LoadFromFileCommand => new DelegateCommand(app.LoadStorageFromFile);

        public DelegateCommand OpenAppLocationCommand => new DelegateCommand(services.OpenAppLocation);

        public DelegateCommand OpenStorageLocationCommand => new DelegateCommand(() =>
        {
            const string openErrorMessage = "Can not open storage file location";
            try
            {
                if (!File.Exists(services.AppSettingsService.StoragePath))
                {
                    services.AppDialogsService.ShowError(openErrorMessage);
                    return;
                }

                var storagePath = services.AppSettingsService.StoragePath;
                services.SysService.OpenFileLocation(storagePath);
            }
            catch (Exception exc)
            {
                services.LogException(openErrorMessage, exc);
            }
        });

        public DelegateCommand CloseCommand => new DelegateCommand(() => OnClose?.Invoke());
        public DelegateCommand ExportToPdfCommand => new DelegateCommand(app.ExportToPdf);
        public DelegateCommand ShowAboutCommand => new DelegateCommand(app.ShowAbout);
        public DelegateCommand ShowExamplesCommand => new DelegateCommand(app.ShowExamples);
        public DelegateCommand AddNewWorkspaceCommand => new DelegateCommand(app.AddNewWorkspace);
        public DelegateCommand AddNewBarcodeCommand => new DelegateCommand(() => app.AddNewBarcode(null, false));
        public DelegateCommand ImportBarcodesCommand => new DelegateCommand(app.ImportBarcodes);
        public DelegateCommand ImportWorkspacesCommand => new DelegateCommand(app.ImportWorkspaces);
        public DelegateCommand ImportStorageCommand => new DelegateCommand(app.ImportStorage);
        public DelegateCommand ShowSettingsCommand => new DelegateCommand(() => services.AppWindowsService.ShowSettingsWindow(null));
        public DelegateCommand CloseAllWindowsCommand => new DelegateCommand(services.AppWindowsService.CloseAllWindows);
        public DelegateCommand ClearCommand => new DelegateCommand(app.Clear);
        public DelegateCommand CreateNewStorageCommand => new DelegateCommand(app.CreateNewStorage);
        public DelegateCommand PrintCommand => new DelegateCommand(app.Print);
        public DelegateCommand QuickGeneratorCommand => new DelegateCommand(services.AppWindowsService.ShowQuickGeneratorWindow);
        public DelegateCommand CloseBarcodesWindowsCommand => new DelegateCommand(services.AppWindowsService.CloseBarcodesWindows);

        public DelegateCommand CloseWorkspacesWindowsCommand => new DelegateCommand(services.AppWindowsService.CloseWorkspacesWindows);
        public DelegateCommand CloseGeneratorsWindowsCommand => new DelegateCommand(services.AppWindowsService.CloseQuickGeneratorsWindows);

        public Action OnClose { get; set; }
    }
}