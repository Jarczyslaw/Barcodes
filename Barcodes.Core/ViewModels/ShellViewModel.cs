using Aspose.BarCode.Generation;
using Barcodes.Core.Services;
using Barcodes.Services.Dialogs;
using Barcodes.Services.Generator;
using Barcodes.Utils;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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

        private string data;
        public string Data
        {
            get => data;
            set => SetProperty(ref data, value);
        }

        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public int BarcodesCount
        {
            get => Barcodes.Count;
        }

        private string statusMessage = string.Empty;
        public string StatusMessage
        {
            get => statusMessage;
            set => SetProperty(ref statusMessage, value);
        }

        public bool ExtraInputEnabled
        {
            get
            {
                if (SelectedBarcodeType == null)
                    return false;
                return SelectedBarcodeType.ExtraInput != null;
            }
        }

        private BarcodeTypeViewModel selectedBarcodeType;
        public BarcodeTypeViewModel SelectedBarcodeType
        {
            get => selectedBarcodeType;
            set
            {
                SetProperty(ref selectedBarcodeType, value);
                ExtraInputCommand.RaiseCanExecuteChanged();
            }
        }

        public BarcodeViewModel SelectedBarcode { get; set; }

        public ObservableCollection<BarcodeTypeViewModel> BarcodeTypes { get; private set; }
        public ObservableCollection<BarcodeViewModel> Barcodes { get; private set; } = new ObservableCollection<BarcodeViewModel>();

        private readonly IBarcodesGeneratorService barcodesGenerator;
        private readonly IDialogsService dialogsService;
        private readonly IBarcodeWindowsService barcodeWindowsService;

        public ShellViewModel(IBarcodesGeneratorService barcodesGenerator, IDialogsService dialogsService, IBarcodeWindowsService barcodeWindowsService)
        {
            this.barcodesGenerator = barcodesGenerator;
            this.dialogsService = dialogsService;
            this.barcodeWindowsService = barcodeWindowsService;

            InitializeBarcodeTypes();
            GenerateRandomBarcode();

            Data = "Test data";
            Title = "Test title";
        }

        private DelegateCommand generateRandomBarcodeCommand;
        public DelegateCommand GenerateRandomBarcodeCommand
        {
            get => generateRandomBarcodeCommand = generateRandomBarcodeCommand ?? new DelegateCommand(GenerateRandomBarcode);
        }

        private DelegateCommand generateBarcodeCommand;
        public DelegateCommand GenerateBarcodeCommand
        {
            get => generateBarcodeCommand = generateBarcodeCommand ?? new DelegateCommand(GenerateBarcode);
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

        private DelegateCommand<BarcodeViewModel> copyToClipboardCommand;
        public DelegateCommand<BarcodeViewModel> CopyToClipboardCommand
        {
            get => copyToClipboardCommand = copyToClipboardCommand ?? new DelegateCommand<BarcodeViewModel>(CopyToClipboard);
        }

        private DelegateCommand<BarcodeViewModel> deleteCommand;
        public DelegateCommand<BarcodeViewModel> DeleteCommand
        {
            get => deleteCommand = deleteCommand ?? new DelegateCommand<BarcodeViewModel>(Delete);
        }

        private void ExtraInput()
        {
            var result = SelectedBarcodeType.ExtraInput();
            if (string.IsNullOrEmpty(result))
                return;
            Data = result;
        }

        private void GenerateRandomBarcode()
        {
            var randomText = RandomTexts.Get();
            RandomBarcode = barcodesGenerator.CreateShellBarcode(400, randomText);
        }

        private void OpenInNewWindow()
        {
            if (SelectedBarcode == null)
                return;

            barcodeWindowsService.OpenBarcodeWindow(SelectedBarcode);
        }

        private void Delete(BarcodeViewModel barcode)
        {
            if (barcode == null)
                return;

            if (!dialogsService.ShowYesNoQuestion("Do you really want to delete selected barcode?"))
                return;

            Barcodes.Remove(barcode);
            RaisePropertyChanged(nameof(BarcodesCount));
        }

        private void CopyToClipboard(BarcodeViewModel barcode)
        {
            if (barcode == null)
                return;

            Clipboard.SetImage(barcode.Barcode);
            StatusMessage = $"Barcode \"{barcode.Title}\" copied to clipboard";
        }

        private void GenerateBarcode()
        {
            if (!GenerateValidation())
                return;

            var barcodeData = new BarcodeData
            {
                MinWidth = 300,
                Data = Data,
                ShowData = false,
                Type = SelectedBarcodeType.Type
            };

            try
            {
                var barcode = barcodesGenerator.CreateBarcode(barcodeData);
                Barcodes.Add(new BarcodeViewModel
                {
                    Barcode = barcode,
                    Data = barcodeData.Data,
                    Title = Title,
                    TypeTitle = SelectedBarcodeType.TypeTitle
                });
                StatusMessage = $"Barcode \"{Title}\" generate successfully!";
                RaisePropertyChanged(nameof(BarcodesCount));
            }
            catch (Exception exc)
            {
                dialogsService.ShowException("Exception during barcode generation", exc);
            }
        }

        private bool GenerateValidation()
        {
            if (string.IsNullOrEmpty(Title))
            {
                dialogsService.ShowError("Enter barcode's title");
                return false;
            }

            if (string.IsNullOrEmpty(Data))
            {
                dialogsService.ShowError("Enter barcode's data");
                return false;
            }

            return true;
        }

        private void InitializeBarcodeTypes()
        {
            BarcodeTypes = new ObservableCollection<BarcodeTypeViewModel>
            {
                new BarcodeTypeViewModel
                {
                    TypeTitle = "Ean13",
                    Type = EncodeTypes.EAN13
                },
                new BarcodeTypeViewModel
                {
                    TypeTitle = "Ean128",
                    Type = EncodeTypes.GS1Code128
                },
                new BarcodeTypeViewModel
                {
                    TypeTitle = "Code128",
                    Type = EncodeTypes.Code128
                },
                new BarcodeTypeViewModel
                {
                    TypeTitle = "DataMatrix",
                    Type = EncodeTypes.DataMatrix,
                    ExtraInput = barcodeWindowsService.OpenNmvsInputWindow
                },
                new BarcodeTypeViewModel
                {
                    TypeTitle = "QR",
                    Type = EncodeTypes.QR
                },
            };
            SelectedBarcodeType = BarcodeTypes.First();
        }
    }
}
