using Barcodes.Core.Common;
using Barcodes.Core.Services;
using Barcodes.Services.AppSettings;
using Prism.Mvvm;
using System;
using Unity;

namespace Barcodes.Core.ViewModels
{
    public class ShellViewModel : BindableBase, IShowDestination, ICloseSource, IClosingDestination
    {
        private readonly IUnityContainer unityContainer;

        public ShellViewModel(IUnityContainer unityContainer)
        {
            this.unityContainer = unityContainer;

            App = unityContainer.Resolve<AppViewModel>();
            Menu = new MenuViewModel(App);
            BarcodeMenu = new BarcodeMenuViewModel(App);
            WorkspaceMenu = new WorkspaceMenuViewModel(App);

            InitialSequence();
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
            if (App.CheckChanges())
            {
                var appDialogService = unityContainer.Resolve<IAppDialogsService>();
                var dialogResult = appDialogService.ShowYesNoQuestion("Do you want to save current storage's changes?");
                if (dialogResult)
                {
                    App.ExecuteSaveToFile();
                }
            }

            return false;
        }

        private void InitialSequence()
        {
            var appSettingsService = unityContainer.Resolve<IAppSettingsService>();
            appSettingsService.Load();
            var storagePath = appSettingsService.StoragePath;
            App.LoadFromFile(storagePath);
        }
    }
}
