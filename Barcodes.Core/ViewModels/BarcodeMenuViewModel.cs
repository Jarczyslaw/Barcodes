using Prism.Commands;
using Prism.Mvvm;

namespace Barcodes.Core.ViewModels
{
    public class BarcodeMenuViewModel : BindableBase
    {
        public BarcodeMenuViewModel(AppViewModel app)
        {
            EditCommand = new DelegateCommand<BarcodeViewModel>(app.EditBarcode);
            MoveUpCommand = new DelegateCommand(app.MoveBarcodesUp);
            MoveDownCommand = new DelegateCommand(app.MoveBarcodesDown);
            OpenInNewWindowCommand = new DelegateCommand(app.OpenBarcodesInNewWindow);
            SaveToImageFileCommand = new DelegateCommand<BarcodeViewModel>(app.SaveToImageFile);
            CopyImageToClipboardCommand = new DelegateCommand<BarcodeViewModel>(app.CopyImageToClipboard);
            CopyDataToClipboardCommand = new DelegateCommand<BarcodeViewModel>(app.CopyDataToClipboard);
            DeleteCommand = new DelegateCommand<BarcodeViewModel>(app.DeleteBarcode);
            ChangeWorkspaceCommand = new DelegateCommand<BarcodeViewModel>(app.ChangeBarcodesWorkspace);
            ExportCommand = new DelegateCommand(app.ExportBarcodes);
            SetAsFirstCommand = new DelegateCommand(app.SetBarcodesAsFirst);
            SetAsLastCommand = new DelegateCommand(app.SetBarcodesAsLast);
        }

        public DelegateCommand<BarcodeViewModel> EditCommand { get; }
        public DelegateCommand MoveUpCommand { get; }
        public DelegateCommand MoveDownCommand { get; }
        public DelegateCommand OpenInNewWindowCommand { get; }
        public DelegateCommand<BarcodeViewModel> SaveToImageFileCommand { get; }
        public DelegateCommand<BarcodeViewModel> CopyImageToClipboardCommand { get; }
        public DelegateCommand<BarcodeViewModel> CopyDataToClipboardCommand { get; }
        public DelegateCommand<BarcodeViewModel> DeleteCommand { get; }
        public DelegateCommand<BarcodeViewModel> ChangeWorkspaceCommand { get; }
        public DelegateCommand ExportCommand { get; }
        public DelegateCommand SetAsFirstCommand { get; }
        public DelegateCommand SetAsLastCommand { get; }
    }
}
