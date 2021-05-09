using Barcodes.Core.Abstraction;
using Barcodes.Core.Common;
using Barcodes.Core.Models;
using Barcodes.Core.Services;
using Barcodes.Services.AppSettings;
using JToolbox.Core.Abstraction;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Barcodes.Core.ViewModels
{
    public class SettingsViewModel : BindableBase, ICloseSource
    {
        private readonly IAppSettingsService appSettingsService;
        private readonly IAppDialogsService appDialogsService;
        private readonly IAppWindowsService appWindowsService;
        private readonly ILoggerService loggerService;
        private readonly IAppEvents appEvents;

        private StartupModeViewModel selectedStartupMode;
        private ObservableCollection<StartupModeViewModel> startupModes = new ObservableCollection<StartupModeViewModel>();
        private int quickBarcodesCount;
        private string storagePath = string.Empty;
        private bool barcodesVisible;
        private string protectedKeys = string.Empty;
        private AddModeViewModel selectedBarcodeAddMode;
        private ObservableCollection<AddModeViewModel> barcodeAddModes = new ObservableCollection<AddModeViewModel>();
        private AddModeViewModel selectedWorkspaceAddMode;
        private ObservableCollection<AddModeViewModel> workspaceAddModes = new ObservableCollection<AddModeViewModel>();
        private GenerationBaseDataViewModel generationData;
        private bool updateAfterEveryGeneration;

        public SettingsViewModel(IAppWindowsService appWindowsService, IAppSettingsService appSettingsService,
            IAppDialogsService appDialogsService, ILoggerService loggerService, IAppEvents appEvents)
        {
            this.appWindowsService = appWindowsService;
            this.appSettingsService = appSettingsService;
            this.appDialogsService = appDialogsService;
            this.loggerService = loggerService;
            this.appEvents = appEvents;

            generationData = new GenerationBaseDataViewModel();
            DragDropModes = new DragDropModesViewModel();

            SaveCommand = new DelegateCommand(Save);
            CloseCommand = new DelegateCommand(() => OnClose?.Invoke());
            SetStoragePathCommand = new DelegateCommand(SetStoragePath);
            RawSettingsCommand = new DelegateCommand(RawSettings);

            InitializeAddModes(BarcodeAddModes);
            InitializeAddModes(WorkspaceAddModes);
            InitializeStartupModes();

            LoadSettings();
        }

        public DelegateCommand SaveCommand { get; }

        public DelegateCommand CloseCommand { get; }

        public DelegateCommand SetStoragePathCommand { get; }

        public DelegateCommand RawSettingsCommand { get; }

        public SettingsSaveResult SettingsSaveResult { get; set; }

        public Action OnClose { get; set; }

        public DragDropModesViewModel DragDropModes { get; }

        public StartupModeViewModel SelectedStartupMode
        {
            get => selectedStartupMode;
            set => SetProperty(ref selectedStartupMode, value);
        }

        public ObservableCollection<StartupModeViewModel> StartupModes
        {
            get => startupModes;
            set => SetProperty(ref startupModes, value);
        }

        public int QuickBarcodesCount
        {
            get => quickBarcodesCount;
            set => SetProperty(ref quickBarcodesCount, value);
        }

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

        public bool UpdateAfterEveryGeneration
        {
            get => updateAfterEveryGeneration;
            set => SetProperty(ref updateAfterEveryGeneration, value);
        }

        public string ProtectedKeys
        {
            get => protectedKeys;
            set => SetProperty(ref protectedKeys, value);
        }

        private AddMode? SelectedBarcodeAddModeRaw
        {
            get => SelectedBarcodeAddMode?.AddMode;
            set => SelectedBarcodeAddMode = BarcodeAddModes.First(b => b.AddMode == value);
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

        private AddMode? SelectedWorkspaceAddModeRaw
        {
            get => SelectedWorkspaceAddMode?.AddMode;
            set => SelectedWorkspaceAddMode = WorkspaceAddModes.First(b => b.AddMode == value);
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

        public GenerationBaseDataViewModel GenerationData
        {
            get => generationData;
            set => SetProperty(ref generationData, value);
        }

        private void InitializeAddModes(ObservableCollection<AddModeViewModel> modes)
        {
            modes.Add(new AddModeViewModel(AddMode.AsFirst));
            modes.Add(new AddModeViewModel(AddMode.AsLast));
        }

        private void InitializeStartupModes()
        {
            startupModes.AddRange(new List<StartupModeViewModel>
            {
                new StartupModeViewModel(StartupMode.DoNothing),
                new StartupModeViewModel(StartupMode.AddNew),
                new StartupModeViewModel(StartupMode.QuickGenerator)
            });
        }

        private void Save()
        {
            var settings = ToSettings();
            var error = settings.Validate();
            if (!string.IsNullOrEmpty(error))
            {
                appDialogsService.ShowError(error);
                return;
            }

            try
            {
                var result = CreateSaveResult(settings);
                appSettingsService.Save(settings);
                SettingsSaveResult = result;

                OnClose?.Invoke();
                appEvents.RiseOnSettingsChanged(result);
                appEvents.RiseOnDragDropModeChanged(settings.DragDropMode);
            }
            catch (Exception exc)
            {
                var message = "Error saving settings";
                loggerService.Error(exc, message);
                appDialogsService.ShowException(exc, message);
            }
        }

        private SettingsSaveResult CreateSaveResult(AppSettings newSettings)
        {
            return new SettingsSaveResult()
            {
                Previous = appSettingsService.AppSettings,
                Current = newSettings
            };
        }

        private void LoadSettings()
        {
            FromSettings(appSettingsService.AppSettings);
        }

        private void FromSettings(AppSettings settings)
        {
            SelectedStartupMode = StartupModes.First(s => s.StartupMode == settings.StartupMode);
            QuickBarcodesCount = settings.QuickBarcodesCount;
            StoragePath = settings.StoragePath;
            BarcodesVisible = settings.BarcodesVisible;
            ProtectedKeys = settings.AntiKeyProtection;
            SelectedBarcodeAddModeRaw = settings.BarcodeAddMode;
            SelectedWorkspaceAddModeRaw = settings.WorkspaceAddMode;
            UpdateAfterEveryGeneration = settings.UpdateAfterEveryGeneration;
            GenerationData.FromSettings(settings.GenerationSettings);
            DragDropModes.Select(settings.DragDropMode);
        }

        private AppSettings ToSettings()
        {
            return new AppSettings
            {
                StartupMode = SelectedStartupMode.StartupMode,
                QuickBarcodesCount = QuickBarcodesCount,
                StoragePath = StoragePath,
                BarcodesVisible = BarcodesVisible,
                AntiKeyProtection = ProtectedKeys,
                BarcodeAddMode = SelectedBarcodeAddModeRaw.Value,
                WorkspaceAddMode = SelectedWorkspaceAddModeRaw.Value,
                UpdateAfterEveryGeneration = UpdateAfterEveryGeneration,
                GenerationSettings = GenerationData.ToSettings(),
                DragDropMode = DragDropModes.SelectedItem.DragDropMode
            };
        }

        private void RawSettings()
        {
            var result = appWindowsService.ShowRawSettingsWindow(this, ToSettings());
            if (result != null)
            {
                FromSettings(result);
            }
        }

        private void SetStoragePath()
        {
            var newStoragePath = appDialogsService.OpenStorageFile(StoragePath);
            if (!string.IsNullOrEmpty(newStoragePath))
            {
                StoragePath = newStoragePath;
            }
        }
    }
}