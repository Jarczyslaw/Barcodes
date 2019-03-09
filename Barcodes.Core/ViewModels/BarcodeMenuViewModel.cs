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
            OpenInNewWindowCommand = new DelegateCommand(app.OpenInNewWindow);
            SaveToImageFileCommand = new DelegateCommand<BarcodeViewModel>(app.SaveToImageFile);
            CopyToClipboardCommand = new DelegateCommand<BarcodeViewModel>(app.CopyToClipboard);
            DeleteCommand = new DelegateCommand<BarcodeViewModel>(app.DeleteBarcode);
            ChangeWorkspaceCommand = new DelegateCommand<BarcodeViewModel>(app.ChangeBarcodesWorkspace);
            ExportCommand = new DelegateCommand<BarcodeViewModel>(app.ExportBarcode);
        }

        public DelegateCommand<BarcodeViewModel> EditCommand { get; }
        public DelegateCommand<BarcodeViewModel> MoveUpCommand { get; }
        public DelegateCommand<BarcodeViewModel> MoveDownCommand { get; }
        public DelegateCommand OpenInNewWindowCommand { get; }
        public DelegateCommand<BarcodeViewModel> SaveToImageFileCommand { get; }
        public DelegateCommand<BarcodeViewModel> CopyToClipboardCommand { get; }
        public DelegateCommand<BarcodeViewModel> DeleteCommand { get; }
        public DelegateCommand<BarcodeViewModel> ChangeWorkspaceCommand { get; }
        public DelegateCommand<BarcodeViewModel> ExportCommand { get; }
    }
}
