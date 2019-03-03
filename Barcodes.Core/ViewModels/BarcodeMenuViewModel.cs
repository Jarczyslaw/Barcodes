using Prism.Commands;
using Prism.Mvvm;

namespace Barcodes.Core.ViewModels
{
    public class BarcodeMenuViewModel : BindableBase
    {
        public BarcodeMenuViewModel(AppViewModel app)
        {
            EditBarcodeCommand = new DelegateCommand<BarcodeResultViewModel>(app.EditBarcode);
            MoveUpCommand = new DelegateCommand<BarcodeResultViewModel>(app.MoveBarcodeUp);
            MoveDownCommand = new DelegateCommand<BarcodeResultViewModel>(app.MoveBarcodeDown);
            OpenInNewWindowCommand = new DelegateCommand(app.OpenInNewWindow);
            SaveToImageFileCommand = new DelegateCommand<BarcodeResultViewModel>(app.SaveToImageFile);
            CopyToClipboardCommand = new DelegateCommand<BarcodeResultViewModel>(app.CopyToClipboard);
            DeleteCommand = new DelegateCommand<BarcodeResultViewModel>(app.DeleteBarcode);
            ChangeBarcodesWorkspaceCommand = new DelegateCommand(app.ChangeBarcodesWorkspace);
            ExportCommand = new DelegateCommand<BarcodeResultViewModel>(app.ExportBarcode);
        }

        public DelegateCommand<BarcodeResultViewModel> EditBarcodeCommand { get; }
        public DelegateCommand<BarcodeResultViewModel> MoveUpCommand { get; }
        public DelegateCommand<BarcodeResultViewModel> MoveDownCommand { get; }
        public DelegateCommand OpenInNewWindowCommand { get; }
        public DelegateCommand<BarcodeResultViewModel> SaveToImageFileCommand { get; }
        public DelegateCommand<BarcodeResultViewModel> CopyToClipboardCommand { get; }
        public DelegateCommand<BarcodeResultViewModel> DeleteCommand { get; }
        public DelegateCommand ChangeBarcodesWorkspaceCommand { get; }
        public DelegateCommand<BarcodeResultViewModel> ExportCommand { get; }
    }
}
