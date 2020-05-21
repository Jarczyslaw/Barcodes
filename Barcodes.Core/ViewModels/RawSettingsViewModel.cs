using Barcodes.Core.Abstraction;
using Barcodes.Core.Common;
using Barcodes.Services.AppSettings;
using Prism.Commands;
using Prism.Mvvm;
using System;

namespace Barcodes.Core.ViewModels
{
    public class RawSettingsViewModel : BindableBase, ICloseSource
    {
        private string settings = string.Empty;

        private readonly IAppDialogsService appDialogsService;

        public RawSettingsViewModel(IAppDialogsService appDialogsService)
        {
            this.appDialogsService = appDialogsService;

            SaveCommand = new DelegateCommand(Save);
            CloseCommand = new DelegateCommand(() => OnClose?.Invoke());
        }

        public DelegateCommand SaveCommand { get; }

        public DelegateCommand CloseCommand { get; }

        public AppSettings EditedSettings { get; set; }

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
                EditedSettings = AppSettings.Deserialize(Settings.Trim());
                OnClose?.Invoke();
            }
            catch (Exception exc)
            {
                appDialogsService.ShowException("Exception during saving settings file", exc);
            }
        }

        public void LoadSettings(AppSettings appSettings)
        {
            Settings = appSettings.Serialize();
        }
    }
}