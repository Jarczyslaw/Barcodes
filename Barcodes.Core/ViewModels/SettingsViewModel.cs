using Barcodes.Core.Abstraction;
using Barcodes.Core.Common;
using Barcodes.Services.AppSettings;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Barcodes.Core.ViewModels
{
    public class SettingsViewModel : BindableBase, ICloseSource
    {
        private readonly IAppSettingsService appSettingsService;
        private readonly IAppDialogsService appDialogsService;

        private string storagePath = string.Empty;
        private bool barcodesVisible;
        private string protectedKeys = string.Empty;
        private AddModeViewModel selectedBarcodeAddMode;
        private ObservableCollection<AddModeViewModel> barcodeAddModes;
        private AddModeViewModel selectedWorkspaceAddMode;
        private ObservableCollection<AddModeViewModel> workspaceAddModes;
        private GenerationDataViewModel generationData = new GenerationDataViewModel();

        public SettingsViewModel(IAppSettingsService appSettingsService, IAppDialogsService appDialogsService)
        {
            this.appSettingsService = appSettingsService;
            this.appDialogsService = appDialogsService;

            SaveCommand = new DelegateCommand(Save);
            CloseCommand = new DelegateCommand(() => OnClose?.Invoke());
            SetStoragePathCommand = new DelegateCommand(SetStoragePath);
            RawSettingsCommand = new DelegateCommand(RawSettings);

            LoadSettings();
        }

        public DelegateCommand SaveCommand { get; }

        public DelegateCommand CloseCommand { get; }

        public DelegateCommand SetStoragePathCommand { get; }

        public DelegateCommand RawSettingsCommand { get; }

        public bool SettingsSaved { get; set; }

        public Action OnClose { get; set; }

        public string StoragePath
        {
            get => storagePath;
            set => SetProperty(ref storagePath, value);
        }

        public bool BarcodesVisible
        {
            get => barcodesVisible;
            set => SetProperty(ref barcodesVisible, value);
        }

        public string ProtectedKeys
        {
            get => protectedKeys;
            set => SetProperty(ref protectedKeys, value);
        }

        public AddModeViewModel SelectedBarcodeAddMode
        {
            get => selectedBarcodeAddMode;
            set => SetProperty(ref selectedBarcodeAddMode, value);
        }

        public ObservableCollection<AddModeViewModel> BarcodeAddModes
        {
            get => barcodeAddModes;
            set => SetProperty(ref barcodeAddModes, value);
        }

        public AddModeViewModel SelectedWorkspaceAddMode
        {
            get => selectedWorkspaceAddMode;
            set => SetProperty(ref selectedWorkspaceAddMode, value);
        }

        public ObservableCollection<AddModeViewModel> WorkspaceAddModes
        {
            get => workspaceAddModes;
            set => SetProperty(ref workspaceAddModes, value);
        }

        public GenerationDataViewModel GenerationData
        {
            get => generationData;
            set => SetProperty(ref generationData, value);
        }

        private void Save()
        {
        }

        private void SelectMode(ObservableCollection<AddModeViewModel> modes, AddModeViewModel selectedMode, AddMode mode)
        {
            selectedMode = modes.First(m => m.AddMode == mode);
        }

        private void LoadSettings()
        {
            var settings = appSettingsService.AppSettings;
            StoragePath = settings.StoragePath;
            BarcodesVisible = settings.BarcodesVisible;
            ProtectedKeys = settings.AntiKeyProtection;
            SelectMode(BarcodeAddModes, SelectedBarcodeAddMode, settings.BarcodeAddMode);
            SelectMode(WorkspaceAddModes, SelectedWorkspaceAddMode, settings.WorkspaceAddMode);
            GenerationData.FromGenerationSettings(settings.GenerationSettings);
        }

        private void RawSettings()
        {
        }

        private void SetStoragePath()
        {

        }
    }
}