using Barcodes.Core.Abstraction;
using Barcodes.Core.Common;
using Prism.Mvvm;
using System;
using System.Windows.Input;

namespace Barcodes.Core.ViewModels
{
    public class ShellViewModel : BindableBase, IOnShowAware, ICloseSource, IOnClosingAware, IOnKeyDownAware, IMinimizeSource
    {
        private readonly IServicesAggregator services;

        public ShellViewModel(IServicesAggregator services)
        {
            this.services = services;

            App = new AppViewModel(services);
            Menu = new MenuViewModel(App, services);
            BarcodeMenu = new BarcodeMenuViewModel(App, services);
            WorkspaceMenu = new WorkspaceMenuViewModel(App, services);

            App.InitialSequence();
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
            if (services.AppSettingsService.OpenQuickGeneratorOnStartup)
            {
                services.AppWindowsService.ShowQuickGeneratorWindow(App);
                //Minimize();
            }
            else
            {
                if (App.BarcodesCount == 0)
                {
                    App.AddNewBarcode(null, false);
                }
            }
        }

        public bool OnClosing()
        {
            return !App.CheckStorageChangesAndSave();
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
    }
}