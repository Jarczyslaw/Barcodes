﻿using Barcodes.Core.Abstraction;
using Barcodes.Core.Common;
using Barcodes.Core.Models;
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
        private readonly IAppWindowsService appWindowsService;

        private string storagePath = string.Empty;
        private bool barcodesVisible;
        private string protectedKeys = string.Empty;
        private AddModeViewModel selectedBarcodeAddMode;
        private ObservableCollection<AddModeViewModel> barcodeAddModes = new ObservableCollection<AddModeViewModel>();
        private AddModeViewModel selectedWorkspaceAddMode;
        private ObservableCollection<AddModeViewModel> workspaceAddModes = new ObservableCollection<AddModeViewModel>();
        private GenerationDataViewModel generationData = new GenerationDataViewModel();
        private bool updateAfterEveryGeneration;

        public SettingsViewModel(IAppWindowsService appWindowsService, IAppSettingsService appSettingsService, IAppDialogsService appDialogsService)
        {
            this.appWindowsService = appWindowsService;
            this.appSettingsService = appSettingsService;
            this.appDialogsService = appDialogsService;

            SaveCommand = new DelegateCommand(Save);
            CloseCommand = new DelegateCommand(() => OnClose?.Invoke());
            SetStoragePathCommand = new DelegateCommand(SetStoragePath);
            RawSettingsCommand = new DelegateCommand(RawSettings);

            InitializeAddModes(BarcodeAddModes);
            InitializeAddModes(WorkspaceAddModes);

            LoadSettings();
        }

        public DelegateCommand SaveCommand { get; }

        public DelegateCommand CloseCommand { get; }

        public DelegateCommand SetStoragePathCommand { get; }

        public DelegateCommand RawSettingsCommand { get; }

        public SettingsSaveResult SettingsSaveResult { get; set; }

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

        public GenerationDataViewModel GenerationData
        {
            get => generationData;
            set => SetProperty(ref generationData, value);
        }

        private void InitializeAddModes(ObservableCollection<AddModeViewModel> modes)
        {
            modes.Add(new AddModeViewModel(AddMode.AsFirst));
            modes.Add(new AddModeViewModel(AddMode.AsLast));
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
                appSettingsService.Save(settings);
                SettingsSaveResult = new SettingsSaveResult();
                OnClose?.Invoke();
            }
            catch (Exception exc)
            {
                appDialogsService.ShowException("Error saving settings", exc);
            }
        }

        private void LoadSettings()
        {
            FromSettings(appSettingsService.AppSettings);
        }

        private void FromSettings(AppSettings settings)
        {
            StoragePath = settings.StoragePath;
            BarcodesVisible = settings.BarcodesVisible;
            ProtectedKeys = settings.AntiKeyProtection;
            SelectedBarcodeAddModeRaw = settings.BarcodeAddMode;
            SelectedWorkspaceAddModeRaw = settings.WorkspaceAddMode;
            UpdateAfterEveryGeneration = settings.UpdateAfterEveryGeneration;
            GenerationData.FromSettings(settings.GenerationSettings);
        }

        private AppSettings ToSettings()
        {
            return new AppSettings
            {
                StoragePath = StoragePath,
                BarcodesVisible = BarcodesVisible,
                AntiKeyProtection = ProtectedKeys,
                BarcodeAddMode = SelectedBarcodeAddModeRaw.Value,
                WorkspaceAddMode = SelectedWorkspaceAddModeRaw.Value,
                UpdateAfterEveryGeneration = UpdateAfterEveryGeneration,
                GenerationSettings = GenerationData.ToSettings()
            };
        }

        private void RawSettings()
        {
            var result = appWindowsService.ShowRawSettingsWindow(ToSettings());
            if (result != null)
            {
                FromSettings(result);
            }
        }

        private void SetStoragePath()
        {
            var newStoragePath = appDialogsService.OpenStorageFile(StoragePath, false);
            if (!string.IsNullOrEmpty(newStoragePath))
            {
                StoragePath = newStoragePath;
            }
        }
    }
}