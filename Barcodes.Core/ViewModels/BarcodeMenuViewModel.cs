using Prism.Commands;
using Prism.Mvvm;

namespace Barcodes.Core.ViewModels
{
    public class BarcodeMenuViewModel : BindableBase
    {
        public BarcodeMenuViewModel(AppViewModel app)
        {
            EditCommand = new DelegateCommand<BarcodeViewModel>(app.EditBarcode);
            MoveUpCommand = new DelegateCommand<BarcodeViewModel>(app.MoveBarcodeUp);
            MoveDownCommand = new DelegateCommand<BarcodeViewModel>(app.MoveBarcodeDown);
            OpenInNewWindowCommand = new DelegateCommand<BarcodeViewModel>(app.OpenBarcodeInNewWindow);
            SaveToImageFileCommand = new DelegateCommand<BarcodeViewModel>(app.SaveToImageFile);
            CopyImageToClipboardCommand = new DelegateCommand<BarcodeViewModel>(app.CopyImageToClipboard);
            CopyDataToClipboardCommand = new DelegateCommand<BarcodeViewModel>(app.CopyDataToClipboard);
            DeleteCommand = new DelegateCommand<BarcodeViewModel>(app.DeleteBarcode);
            ChangeWorkspaceCommand = new DelegateCommand<BarcodeViewModel>(app.ChangeBarcodesWorkspace);
            ExportCommand = new DelegateCommand<BarcodeViewModel>(app.ExportBarcode);
            SetAsFirstCommand = new DelegateCommand<BarcodeViewModel>(app.SetBarcodeAsFirst);
            SetAsLastCommand = new DelegateCommand<BarcodeViewModel>(app.SetBarcodeAsLast);
        }

        public DelegateCommand<BarcodeViewModel> EditCommand { get; }
        public DelegateCommand<BarcodeViewModel> MoveUpCommand { get; }
        public DelegateCommand<BarcodeViewModel> MoveDownCommand { get; }
        public DelegateCommand<BarcodeViewModel> OpenInNewWindowCommand { get; }
        public DelegateCommand<BarcodeViewModel> SaveToImageFileCommand { get; }
        public DelegateCommand<BarcodeViewModel> CopyImageToClipboardCommand { get; }
        public DelegateCommand<BarcodeViewModel> CopyDataToClipboardCommand { get; }
        public DelegateCommand<BarcodeViewModel> DeleteCommand { get; }
        public DelegateCommand<BarcodeViewModel> ChangeWorkspaceCommand { get; }
        public DelegateCommand<BarcodeViewModel> ExportCommand { get; }
        public DelegateCommand<BarcodeViewModel> SetAsFirstCommand { get; }
        public DelegateCommand<BarcodeViewModel> SetAsLastCommand { get; }
    }
}
