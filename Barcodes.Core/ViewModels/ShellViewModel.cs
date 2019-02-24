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

            AppState = unityContainer.Resolve<IAppState>();
            Menu = unityContainer.Resolve<MenuViewModel>();
            BarcodeMenu = unityContainer.Resolve<BarcodeMenuViewModel>();

            InitialSequence();
        }

        public IAppState AppState { get; }
        public MenuViewModel Menu { get; }
        public BarcodeMenuViewModel BarcodeMenu { get; }

        public Action OnClose
        {
            get => Menu.OnClose;
            set => Menu.OnClose = value;
        }

        public void OnShow()
        {
            if (AppState.BarcodesCount == 0)
            {
                BarcodeMenu.AddNewBarcode();
            }
        }

        private void InitialSequence()
        {
            appSettingsService.Load();
            var storagePath = appSettingsService.StoragePath;
            Menu.LoadBarcodesFromFile(storagePath);
        }
    }
}
