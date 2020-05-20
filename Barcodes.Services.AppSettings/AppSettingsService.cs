using Barcodes.Core.Common;
using Barcodes.Utils;
using System;
using System.IO;

namespace Barcodes.Services.AppSettings
{
    public class AppSettingsService : IAppSettingsService
    {
        public string StoragePath
        {
            get => AppSettings.StoragePath;
            set
            {
                AppSettings.StoragePath = value;
                Save();
            }
        }

        public bool BarcodesVisible
        {
            get => AppSettings.BarcodesVisible;
            set
            {
                AppSettings.BarcodesVisible = value;
                Save();
            }
        }

        public AddMode BarcodeAddMode
        {
            get => AppSettings.BarcodeAddMode;
            set
            {
                AppSettings.BarcodeAddMode = value;
                Save();
            }
        }

        public AddMode WorkspaceAddMode
        {
            get => AppSettings.WorkspaceAddMode;
            set
            {
                AppSettings.WorkspaceAddMode = value;
                Save();
            }
        }

        public string AntiKeyProtection
        {
            get => AppSettings.AntiKeyProtection;
            set
            {
                AppSettings.AntiKeyProtection = value;
                Save();
            }
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

        public void Save()
        {
            Serializer.ToFile(AppSettings, AppSettingsPath);
        }

        public void Save(string settings)
        {
            AppSettings = Serializer.FromString<AppSettings>(settings);
            Save();
        }
    }
}