﻿using Barcodes.Utils;
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

        public string AppSettingsPath { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appSettings.json");

        public AppSettings AppSettings { get; private set; } = new AppSettings();

        public void Load(bool throwException = false)
        {
            try
            {
                AppSettings = Serializer.FromFile<AppSettings>(AppSettingsPath);
            }
            catch
            {
                if (throwException)
                {
                    throw;
                }
            }
        }

        public void Save()
        {
            Serializer.ToFile(AppSettings, AppSettingsPath);
        }
    }
}
