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

        public bool ExtraInputEnabled
        {
            get
            {
                if (Barcodes.SelectedBarcodeType == null)
                    return false;
                return Barcodes.SelectedBarcodeType.ExtraInput != null;
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

            GenerateRandomBarcode();
        }

        private DelegateCommand generateRandomBarcodeCommand;
        public DelegateCommand GenerateRandomBarcodeCommand
        {
            get => generateRandomBarcodeCommand = generateRandomBarcodeCommand ?? new DelegateCommand(GenerateRandomBarcode);
        }

        private DelegateCommand generateBarcodeCommand;
        public DelegateCommand GenerateBarcodeCommand
        {
            get => generateBarcodeCommand = generateBarcodeCommand ?? new DelegateCommand(Barcodes.GenerateBarcode);
        }

        private DelegateCommand extraInputCommand;
        public DelegateCommand ExtraInputCommand
        {
            get => extraInputCommand = extraInputCommand ?? new DelegateCommand(ExtraInput, () => ExtraInputEnabled);
        }

        private DelegateCommand openInNewWindowCommand;
        public DelegateCommand OpenInNewWindowCommand
        {
            get => openInNewWindowCommand = openInNewWindowCommand ?? new DelegateCommand(OpenInNewWindow);
        }

        private DelegateCommand<BarcodeResultViewModel> copyToClipboardCommand;
        public DelegateCommand<BarcodeResultViewModel> CopyToClipboardCommand
        {
            get => copyToClipboardCommand = copyToClipboardCommand ?? new DelegateCommand<BarcodeResultViewModel>(CopyToClipboard);
        }

        private DelegateCommand<BarcodeResultViewModel> deleteCommand;
        public DelegateCommand<BarcodeResultViewModel> DeleteCommand
        {
            get => deleteCommand = deleteCommand ?? new DelegateCommand<BarcodeResultViewModel>(Barcodes.Delete);
        }

        private void ExtraInput()
        {
            var result = SelectedBarcodeType.ExtraInput();
            if (string.IsNullOrEmpty(result))
                return;

            Barcodes.Data = result;
        }

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
