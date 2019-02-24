using Prism.Commands;
using Prism.Mvvm;

namespace Barcodes.Core.ViewModels
{
    public class BarcodeMenuViewModel : BindableBase
    {
        public BarcodeMenuViewModel(AppViewModel appViewModel)
        {
            AddNewBarcodeCommand = new DelegateCommand(appViewModel.AddNewBarcode);
            EditBarcodeCommand = new DelegateCommand<BarcodeResultViewModel>(appViewModel.EditBarcode);
            MoveUpCommand = new DelegateCommand<BarcodeResultViewModel>(appViewModel.MoveUp);
            MoveDownCommand = new DelegateCommand<BarcodeResultViewModel>(appViewModel.MoveDown);
            OpenInNewWindowCommand = new DelegateCommand(appViewModel.OpenInNewWindow);
            SaveToImageFileCommand = new DelegateCommand<BarcodeResultViewModel>(appViewModel.SaveToImageFile);
            CopyToClipboardCommand = new DelegateCommand<BarcodeResultViewModel>(appViewModel.CopyToClipboard);
            DeleteCommand = new DelegateCommand<BarcodeResultViewModel>(appViewModel.Delete);
        }

        public DelegateCommand AddNewBarcodeCommand { get; }
        public DelegateCommand<BarcodeResultViewModel> EditBarcodeCommand { get; }
        public DelegateCommand<BarcodeResultViewModel> MoveUpCommand { get; }
        public DelegateCommand<BarcodeResultViewModel> MoveDownCommand { get; }
        public DelegateCommand OpenInNewWindowCommand { get; }
        public DelegateCommand<BarcodeResultViewModel> SaveToImageFileCommand { get; }
        public DelegateCommand<BarcodeResultViewModel> CopyToClipboardCommand { get; }
        public DelegateCommand<BarcodeResultViewModel> DeleteCommand { get; }
    }
}
