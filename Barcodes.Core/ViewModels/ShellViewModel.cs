using Barcodes.Core.Common;
using Barcodes.Core.Services;
using Prism.Mvvm;
using System;

namespace Barcodes.Core.ViewModels
{
    public class ShellViewModel : BindableBase, IOnShowAware, ICloseSource, IOnClosingAware
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
                App.AddNewBarcode();
            }
        }

        public bool OnClosing()
        {
            if (App.CheckStorageChanges())
            {
                var closingMode = servicesContainer.AppDialogsService.ShowClosingQuestion();
                if (closingMode == ClosingMode.SaveChanges)
                {
                    App.SaveToFile(false);
                }
                else if (closingMode == ClosingMode.Cancel)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
