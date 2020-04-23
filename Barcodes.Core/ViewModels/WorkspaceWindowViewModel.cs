using Barcodes.Core.Services;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace Barcodes.Core.ViewModels
{
    public class WorkspaceWindowViewModel : BindableBase
    {
        private readonly IAppWindowsService appWindowsService;

        public WorkspaceWindowViewModel(WorkspaceViewModel workspaceViewModel, IAppWindowsService appWindowsService)
        {
            this.appWindowsService = appWindowsService;

            Name = workspaceViewModel.Name;
            Barcodes = new ObservableCollection<BarcodeViewModel>(workspaceViewModel.Barcodes);
        }

        public DelegateCommand<BarcodeViewModel> OpenInNewWindowCommand => new DelegateCommand<BarcodeViewModel>(barcode => appWindowsService.OpenBarcodeWindow(barcode));

        public string Name { get; }
        public string HeaderTitle => $"Workspace - {Name}";
        public ObservableCollection<BarcodeViewModel> Barcodes { get; }
    }
}