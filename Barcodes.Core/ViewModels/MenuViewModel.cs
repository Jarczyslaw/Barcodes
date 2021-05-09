using Barcodes.Core.Abstraction;
using Barcodes.Core.Models;
using Barcodes.Services.AppSettings;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Threading.Tasks;

namespace Barcodes.Core.ViewModels
{
    public class MenuViewModel : BindableBase
    {
        private readonly IServicesAggregator services;
        private readonly AppViewModel app;
        private bool barcodesVisible;

        public MenuViewModel(AppViewModel app, IServicesAggregator services)
        {
            this.services = services;
            this.app = app;

            DragDropModes = new DragDropModesViewModel();
            DragDropModes.DragDropModeChanged += DragDropModes_DragDropModeChanged;
            services.AppEvents.OnSettingsChanged += AppEvents_SettingsChanged;
        }

        public DelegateCommand SaveCommand => new DelegateCommand(() => app.Save(false, false));
        public DelegateCommand SaveAsCommand => new DelegateCommand(() => app.Save(true, false));
        public DelegateCommand LoadFromFileCommand => new DelegateCommand(async () => await app.LoadStorageFromFile());

        public DelegateCommand OpenAppLocationCommand => new DelegateCommand(services.OpenAppLocation);

        public DelegateCommand OpenStorageLocationCommand => new DelegateCommand(() => services.OpenStorageLocation(services.AppSettingsService.StoragePath));

        public DelegateCommand CloseCommand => new DelegateCommand(() => OnClose?.Invoke());
        public DelegateCommand ExportCommand => new DelegateCommand(app.Export);
        public DelegateCommand ExportToPdfCommand => new DelegateCommand(app.ExportToPdf);
        public DelegateCommand ShowAboutCommand => new DelegateCommand(async () => await app.ShowAbout());
        public DelegateCommand ShowExamplesCommand => new DelegateCommand(async () => await app.ShowExamples());
        public DelegateCommand AddNewWorkspaceCommand => new DelegateCommand(app.AddNewWorkspace);
        public DelegateCommand AddNewBarcodeCommand => new DelegateCommand(() => app.AddNewBarcode(null, false));
        public DelegateCommand ImportBarcodesCommand => new DelegateCommand(app.ImportBarcodes);
        public DelegateCommand ImportWorkspacesCommand => new DelegateCommand(app.ImportWorkspaces);
        public DelegateCommand ImportStorageCommand => new DelegateCommand(async () => await app.ImportStorage());
        public DelegateCommand ShowSettingsCommand => new DelegateCommand(() => services.AppWindowsService.ShowSettingsWindow(null));
        public DelegateCommand CloseAllWindowsCommand => new DelegateCommand(services.AppWindowsService.CloseAllWindows);
        public DelegateCommand ClearCommand => new DelegateCommand(app.Clear);
        public DelegateCommand CreateNewStorageCommand => new DelegateCommand(async () => await app.CreateNewStorage());
        public DelegateCommand PrintCommand => new DelegateCommand(app.Print);
        public DelegateCommand QuickGeneratorCommand => new DelegateCommand(() => services.AppWindowsService.ShowQuickGeneratorWindow(app));
        public DelegateCommand CloseBarcodesWindowsCommand => new DelegateCommand(services.AppWindowsService.CloseBarcodesWindows);

        public DelegateCommand CloseWorkspacesWindowsCommand => new DelegateCommand(services.AppWindowsService.CloseWorkspacesWindows);
        public DelegateCommand CloseGeneratorsWindowsCommand => new DelegateCommand(services.AppWindowsService.CloseQuickGeneratorsWindows);

        public DelegateCommand OpenLogsCommand => new DelegateCommand(services.OpenLogs);

        public DragDropModesViewModel DragDropModes { get; }

        public bool BarcodesVisible
        {
            get => barcodesVisible;
            set
            {
                SetProperty(ref barcodesVisible, value);
                services.AppSettingsService.BarcodesVisible = value;
            }
        }

        public Action OnClose { get; set; }

        private void ApplySettings(AppSettings appSettings)
        {
            BarcodesVisible = appSettings.BarcodesVisible;
            DragDropModes.Select(appSettings.DragDropMode);
        }

        public Task ApplyInitialSettings(AppSettings appSettings)
        {
            ApplySettings(appSettings);
            return Task.CompletedTask;
        }

        private void AppEvents_SettingsChanged(SettingsSaveResult settingsSaveResult)
        {
            ApplySettings(settingsSaveResult.Current);
        }

        private void DragDropModes_DragDropModeChanged(DragDropMode mode)
        {
            services.AppSettingsService.DragDropMode = mode;
            services.AppEvents.RiseOnDragDropModeChanged(mode);
        }
    }
}