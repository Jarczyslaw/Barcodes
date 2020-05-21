﻿namespace Barcodes.Services.AppSettings
{
    public interface IAppSettingsService
    {
        string StoragePath { get; set; }
        bool BarcodesVisible { get; set; }
        AddMode BarcodeAddMode { get; set; }
        AddMode WorkspaceAddMode { get; set; }
        string AntiKeyProtection { get; set; }

        AppSettings AppSettings { get; }
        string AppSettingsPath { get; }

        void Load(bool throwException = false);

        void Save();

        void Save(AppSettings appSettings);

        void Save(string settings);
    }
}