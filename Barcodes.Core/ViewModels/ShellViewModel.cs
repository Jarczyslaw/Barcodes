using Barcodes.Core.Common;
using Barcodes.Core.Services;
using Barcodes.Services.AppSettings;
using Prism.Mvvm;
using Unity;

namespace Barcodes.Core.ViewModels
{
    public class ShellViewModel : BindableBase, IShownAware
    {
        private readonly IAppSettingsService appSettingsService;

        public ShellViewModel(IUnityContainer unityContainer, IAppSettingsService appSettingsService)
        {
            this.appSettingsService = appSettingsService;

            AppState = unityContainer.Resolve<IAppState>();
            Menu = unityContainer.Resolve<MenuViewModel>();
            ContextMenu = unityContainer.Resolve<ContextMenuViewModel>();

            InitialSequence();
        }

        public IAppState AppState { get; }
        public MenuViewModel Menu { get; }
        public ContextMenuViewModel ContextMenu { get; }

        public void OnViewShown()
        {
            if (AppState.BarcodesCount == 0)
            {
                Menu.AddNewBarcode();
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
