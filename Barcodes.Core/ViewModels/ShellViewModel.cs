using Barcodes.Core.Abstraction;
using Barcodes.Core.Common;
using Barcodes.Core.Models;
using Barcodes.Services.AppSettings;
using Prism.Mvvm;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Barcodes.Core.ViewModels
{
    public class ShellViewModel : BindableBase, IOnShowAware, ICloseSource, IOnClosingAware, IOnKeyDownAware, IMinimizeSource, IOnInitilizedAware
    {
        private readonly IServicesAggregator services;

        public ShellViewModel(IServicesAggregator services)
        {
            this.services = services;

            App = new AppViewModel(services);
            Menu = new MenuViewModel(App, services);
            BarcodeMenu = new BarcodeMenuViewModel(App, services);
            WorkspaceMenu = new WorkspaceMenuViewModel(App, services);
        }

        public AppViewModel App { get; }
        public MenuViewModel Menu { get; }
        public BarcodeMenuViewModel BarcodeMenu { get; }
        public WorkspaceMenuViewModel WorkspaceMenu { get; }

        public Action OnClose
        {
            get => Menu.OnClose;
            set => Menu.OnClose = value;
        }

        public Action Minimize { get; set; }

        public void OnShow()
        {
            var mode = services.AppSettingsService.StartupMode;
            if (mode == StartupMode.QuickGenerator)
            {
                services.AppWindowsService.ShowQuickGeneratorWindow(App);
            }
            else if (mode == StartupMode.AddNew && App.BarcodesCount == 0)
            {
                App.AddNewBarcode(null, false);
            }
        }

        public bool OnClosing()
        {
            return App.CheckStorageChangesAndSave();
        }

        public bool OnKeyDown(KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == Key.Delete)
            {
                App.DeleteBarcodes();
                return true;
            }
            return false;
        }

        public void OnInitilized()
        {
            if (services.AppSettingsService.StartupMode == StartupMode.QuickGenerator)
            {
                Minimize();
            }
        }

        public async Task InitialSequence()
        {
            try
            {
                services.AppSettingsService.Load(false);
                var settings = services.AppSettingsService.AppSettings;
                await App.ApplyInitialSettings(settings);
                await Menu.ApplyInitialSettings(settings);
                services.AppEvents.RiseOnSettingsChanged(new SettingsSaveResult
                {
                    InitialLoad = true,
                    Current = settings,
                    Previous = settings
                });
            }
            catch (Exception exc)
            {
                services.LogException("Error while loading storage from file", exc);
            }
        }
    }
}