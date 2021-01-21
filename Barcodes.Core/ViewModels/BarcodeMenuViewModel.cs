using Barcodes.Core.Abstraction;
using Prism.Commands;
using Prism.Mvvm;

namespace Barcodes.Core.ViewModels
{
    public class BarcodeMenuViewModel : BindableBase
    {
        private readonly IServicesAggregator services;
        private readonly AppViewModel app;

        public BarcodeMenuViewModel(AppViewModel app, IServicesAggregator services)
        {
            this.services = services;
            this.app = app;
        }

        public DelegateCommand<BarcodeViewModel> EditCommand => new DelegateCommand<BarcodeViewModel>(app.EditBarcode);
        public DelegateCommand MoveUpCommand => new DelegateCommand(app.MoveBarcodesUp);
        public DelegateCommand MoveDownCommand => new DelegateCommand(app.MoveBarcodesDown);
        public DelegateCommand OpenInNewWindowCommand => new DelegateCommand(app.OpenBarcodesInNewWindow);
        public DelegateCommand<BarcodeViewModel> SaveToImageFileCommand => new DelegateCommand<BarcodeViewModel>(app.SaveToImageFile);
        public DelegateCommand<BarcodeViewModel> CopyImageToClipboardCommand => new DelegateCommand<BarcodeViewModel>(app.CopyImageToClipboard);
        public DelegateCommand<BarcodeViewModel> CopyDataToClipboardCommand => new DelegateCommand<BarcodeViewModel>(app.CopyDataToClipboard);
        public DelegateCommand DeleteCommand => new DelegateCommand(app.DeleteBarcodes);
        public DelegateCommand ChangeWorkspaceCommand => new DelegateCommand(app.ChangeBarcodesWorkspace);
        public DelegateCommand ExportCommand => new DelegateCommand(app.ExportBarcodes);
        public DelegateCommand SetAsFirstCommand => new DelegateCommand(app.SetBarcodesAsFirst);
        public DelegateCommand SetAsLastCommand => new DelegateCommand(app.SetBarcodesAsLast);
    }
}