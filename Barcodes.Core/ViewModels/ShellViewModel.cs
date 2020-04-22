using Barcodes.Core.Common;
using Barcodes.Core.Services;
using Prism.Mvvm;
using System;
using System.Windows.Input;

namespace Barcodes.Core.ViewModels
{
    public class ShellViewModel : BindableBase, IOnShowAware, ICloseSource, IOnClosingAware, IOnKeyDownAware
    {
        private readonly IServicesContainer servicesContainer;

        public ShellViewModel(IServicesContainer servicesContainer)
        {
            this.servicesContainer = servicesContainer;

            App = new AppViewModel(servicesContainer);
            Menu = new MenuViewModel(App);
            BarcodeMenu = new BarcodeMenuViewModel(App);
            WorkspaceMenu = new WorkspaceMenuViewModel(App);

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

        public void OnShow()
        {
            if (App.BarcodesCount == 0)
            {
                App.AddNewBarcode(null, false);
            }
        }

        public bool OnClosing()
        {
            return App.CheckStorageSave();
        }

        public bool OnKeyDown(KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == Key.Delete)
            {
                App.TryDeleteSelectedBarcode();
                return true;
            }
            return false;
        }
    }
}
