using System;
using Barcodes.Core.Common;
using Barcodes.Core.Services;
using Barcodes.Services.AppSettings;
using Prism.Mvvm;
using Unity;

namespace Barcodes.Core.ViewModels
{
    public class ShellViewModel : BindableBase, IShowAware, ICloseAware
    {
        private readonly IAppSettingsService appSettingsService;

        public ShellViewModel(IUnityContainer unityContainer, IAppSettingsService appSettingsService)
        {
            this.appSettingsService = appSettingsService;

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
            if (App.IsEmpty)
            {
                App.AddNewBarcode();
            }
        }

        private void InitialSequence()
        {
            appSettingsService.Load();
            var storagePath = appSettingsService.StoragePath;
            App.LoadBarcodesFromFile(storagePath);
        }
    }
}
