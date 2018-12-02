using Barcodes.Core.Services;
using Barcodes.Services.Dialogs;
using Barcodes.Services.Generator;
using Barcodes.Utils;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Barcodes.Core.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private BitmapSource randomBarcode;
        public BitmapSource RandomBarcode
        {
            get => randomBarcode;
            set => SetProperty(ref randomBarcode, value);
        }

        public BarcodeTypeViewModel SelectedBarcodeType
        {
            get => Barcodes.SelectedBarcodeType;
            set
            {
                Barcodes.SelectedBarcodeType = value;
                RaisePropertyChanged();
                ExtraInputCommand.RaiseCanExecuteChanged();
            }
        }

        private BarcodesViewModel barcodes;
        public BarcodesViewModel Barcodes
        {
            get => barcodes;
            set => SetProperty(ref barcodes, value);
        }

        private readonly IBarcodesGeneratorService barcodesGenerator;
        private readonly IDialogsService dialogsService;
        private readonly IBarcodeWindowsService barcodeWindowsService;

        public ShellViewModel(IBarcodesGeneratorService barcodesGenerator, IDialogsService dialogsService, IBarcodeWindowsService barcodeWindowsService)
        {
            this.barcodesGenerator = barcodesGenerator;
            this.dialogsService = dialogsService;
            this.barcodeWindowsService = barcodeWindowsService;

            Barcodes = new BarcodesViewModel(barcodesGenerator, dialogsService, barcodeWindowsService);

            GenerateRandomBarcodeCommand = new DelegateCommand(GenerateRandomBarcode);
            GenerateBarcodeCommand = new DelegateCommand(Barcodes.GenerateBarcode);
            ExtraInputCommand = new DelegateCommand(Barcodes.ExtraInput, () => Barcodes.ExtraInputEnabled);
            OpenInNewWindowCommand = new DelegateCommand(OpenInNewWindow);
            CopyToClipboardCommand = new DelegateCommand<BarcodeResultViewModel>(CopyToClipboard);
            DeleteCommand = new DelegateCommand<BarcodeResultViewModel>(Barcodes.Delete);

            GenerateRandomBarcode();
        }

        public DelegateCommand GenerateRandomBarcodeCommand { get; private set; }
        public DelegateCommand GenerateBarcodeCommand { get; private set; }
        public DelegateCommand ExtraInputCommand { get; private set; }
        public DelegateCommand OpenInNewWindowCommand { get; private set; }
        public DelegateCommand<BarcodeResultViewModel> CopyToClipboardCommand { get; private set; }
        public DelegateCommand<BarcodeResultViewModel> DeleteCommand { get; private set; }

        private void GenerateRandomBarcode()
        {
            var randomText = RandomTexts.Get();
            RandomBarcode = barcodesGenerator.CreateShellBarcode(400, randomText);
        }

        private void OpenInNewWindow()
        {
            if (Barcodes.SelectedBarcode == null)
                return;

            barcodeWindowsService.OpenBarcodeWindow(Barcodes.SelectedBarcode);
        }

        private void CopyToClipboard(BarcodeResultViewModel barcode)
        {
            if (barcode == null)
                return;

            Clipboard.SetImage(barcode.Barcode);
            Barcodes.StatusMessage = $"Barcode \"{barcode.Title}\" copied to clipboard";
        }
    }
}
