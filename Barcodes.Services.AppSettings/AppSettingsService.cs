using Barcodes.Core.Common;
using Barcodes.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace Barcodes.Services.AppSettings
{
    public class AppSettingsService : IAppSettingsService
    {
        public StartupMode StartupMode
        {
            get => AppSettings.StartupMode;
            set => SetAndSave(value, AppSettings.StartupMode,
                () => AppSettings.StartupMode = value);
        }

        public int QuickBarcodesCount
        {
            get => AppSettings.QuickBarcodesCount;
            set => SetAndSave(value, AppSettings.QuickBarcodesCount,
                () => AppSettings.QuickBarcodesCount = value);
        }

        public string StoragePath
        {
            get => AppSettings.StoragePath;
            set => SetAndSave(value, AppSettings.StoragePath,
                () => AppSettings.StoragePath = value);
        }

        public bool BarcodesVisible
        {
            get => AppSettings.BarcodesVisible;
            set => SetAndSave(value, AppSettings.BarcodesVisible,
                () => AppSettings.BarcodesVisible = value);
        }

        public AddMode BarcodeAddMode
        {
            get => AppSettings.BarcodeAddMode;
            set => SetAndSave(value, AppSettings.BarcodeAddMode,
                () => AppSettings.BarcodeAddMode = value);
        }

        public AddMode WorkspaceAddMode
        {
            get => AppSettings.WorkspaceAddMode;
            set => SetAndSave(value, AppSettings.WorkspaceAddMode,
                () => AppSettings.WorkspaceAddMode = value);
        }

        public string AntiKeyProtection
        {
            get => AppSettings.AntiKeyProtection;
            set => SetAndSave(value, AppSettings.AntiKeyProtection,
                () => AppSettings.AntiKeyProtection = value);
        }

        public GenerationSettings GenerationSettings
        {
            get => AppSettings.GenerationSettings;
            set => SetAndSave(value, AppSettings.GenerationSettings,
                () => AppSettings.GenerationSettings = value);
        }

        public bool UpdateAfterEveryGeneration
        {
            get => AppSettings.UpdateAfterEveryGeneration;
            set => SetAndSave(value, AppSettings.UpdateAfterEveryGeneration,
                () => AppSettings.UpdateAfterEveryGeneration = value);
        }

        public DragDropMode DragDropMode
        {
            get => AppSettings.DragDropMode;
            set => SetAndSave(value, AppSettings.DragDropMode,
                () => AppSettings.DragDropMode = value);
        }

        public string AppSettingsPath { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"appSettings.{FileExtensions.Settings}");

        public AppSettings AppSettings { get; private set; } = new AppSettings();

        public void Load(bool throwException = false)
        {
            try
            {
                AppSettings = Serializer.FromFile<AppSettings>(AppSettingsPath);
            }
            catch when (!throwException)
            {
            }
        }

        public void Save(AppSettings appSettings)
        {
            AppSettings = appSettings;
            Save();
        }

        public void Save()
        {
            Serializer.ToFile(AppSettings, AppSettingsPath);
        }

        public void Save(string settings)
        {
            AppSettings = Serializer.FromString<AppSettings>(settings);
            Save();
        }

        public void TryUpdateGenerationSettings(GenerationSettings generationSettings)
        {
            if (UpdateAfterEveryGeneration)
            {
                GenerationSettings = generationSettings;
            }
        }

        private void SetAndSave<T>(T newValue, T oldValue, Action setter)
        {
            if (!EqualityComparer<T>.Default.Equals(newValue, oldValue))
            {
                setter();
                Save();
            }
        }
    }
}