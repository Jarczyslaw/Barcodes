using Barcodes.Core.Common;
using Barcodes.Core.Services;
using Barcodes.Services.AppSettings;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.IO;

namespace Barcodes.Core.ViewModels
{
    public class SettingsViewModel : BindableBase, ICloseSource
    {
        private int width = 600;
        private int height = 500;
        private string settings;

        private readonly IAppSettingsService appSettingsService;
        private readonly IAppDialogsService appDialogsService;

        public SettingsViewModel(IAppSettingsService appSettingsService, IAppDialogsService appDialogsService)
        {
            this.appSettingsService = appSettingsService;
            this.appDialogsService = appDialogsService;

            SaveCommand = new DelegateCommand(Save);
            CloseCommand = new DelegateCommand(() => OnClose?.Invoke());

            LoadSettings();
        }

        public DelegateCommand SaveCommand { get; }

        public DelegateCommand CloseCommand { get; }

        public bool SettingsSaved { get; set; }

        public int Width
        {
            get => width;
            set => SetProperty(ref width, value);
        }

        public int Height
        {
            get => height;
            set => SetProperty(ref height, value);
        }

        public string Settings
        {
            get => settings;
            set => SetProperty(ref settings, value);
        }

        public Action OnClose { get; set; }

        private void Save()
        {
            try
            {
                appSettingsService.Save(Settings);
                SettingsSaved = true;
                appDialogsService.ShowInfo("Settings successfully saved");
                OnClose?.Invoke();
            }
            catch (Exception exc)
            {
                appDialogsService.ShowException("Exception during saving settings file", exc);
            }
        }

        private void LoadSettings()
        {
            try
            {
                Settings = File.ReadAllText(appSettingsService.AppSettingsPath);
            }
            catch (Exception exc)
            {
                appDialogsService.ShowException("Exception during loading settings file", exc);
            }
        }
    }
}