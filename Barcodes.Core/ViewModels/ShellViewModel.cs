using Barcodes.Core.Services;
using Prism.Mvvm;
using Unity;

namespace Barcodes.Core.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        public ShellViewModel(IUnityContainer unityContainer)
        {
            AppState = unityContainer.Resolve<IAppState>();
            Menu = unityContainer.Resolve<MenuViewModel>();
            ContextMenu = unityContainer.Resolve<ContextMenuViewModel>();
        }

        public IAppState AppState { get; }
        public MenuViewModel Menu { get; }
        public ContextMenuViewModel ContextMenu { get; }
    }
}
