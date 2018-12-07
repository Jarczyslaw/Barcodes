using Aspose.BarCode.Generation;
using Barcodes.Core.Services;
using Barcodes.Services.Dialogs;
using Barcodes.Services.Generator;
using Barcodes.Services.System;
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
    public class BarcodesViewModel : BindableBase
    {
        private string statusMessage = string.Empty;
        public string StatusMessage
        {
            get => statusMessage;
            set => SetProperty(ref statusMessage, value);
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

        public bool AdditionalInputEnabled
        {
            get
            {
                if (SelectedBarcodeType == null)
                    return false;
                return SelectedBarcodeType.AdditionalInput != null;
            }
        }

        private BarcodeTypeViewModel selectedBarcodeType;
        public BarcodeTypeViewModel SelectedBarcodeType
        {
            get => selectedBarcodeType;
            set
            {
                SetProperty(ref selectedBarcodeType, value);
                AdditionalInputCommand.RaiseCanExecuteChanged();
            }
        }

        private BarcodeResultViewModel selectedBarcode;
        public BarcodeResultViewModel SelectedBarcode
        {
            get => selectedBarcode;
            set => SetProperty(ref selectedBarcode, value);
        }

        public ObservableCollection<BarcodeTypeViewModel> BarcodeTypes { get; private set; } = new ObservableCollection<BarcodeTypeViewModel>();
        public ObservableCollection<BarcodeResultViewModel> Barcodes { get; private set; } = new ObservableCollection<BarcodeResultViewModel>();

        private readonly IBarcodesGeneratorService barcodesGenerator;
        private readonly IAppDialogsService dialogsService;
        private readonly IAppWindowsService appWindowsService;
        private readonly ISystemService systemService;

        public BarcodesViewModel(IBarcodesGeneratorService barcodesGenerator, IAppDialogsService dialogsService,
            IAppWindowsService appWindowsService, ISystemService systemService)
        {
            this.barcodesGenerator = barcodesGenerator;
            this.dialogsService = dialogsService;
            this.appWindowsService = appWindowsService;
            this.systemService = systemService;

            GenerateBarcodeCommand = new DelegateCommand(GenerateBarcode);
            AdditionalInputCommand = new DelegateCommand(AdditionalInput, () => AdditionalInputEnabled);
            OpenInNewWindowCommand = new DelegateCommand(OpenInNewWindow);
            CopyToClipboardCommand = new DelegateCommand<BarcodeResultViewModel>(CopyToClipboard);
            DeleteCommand = new DelegateCommand<BarcodeResultViewModel>(Delete);

            InitializeBarcodeTypes();

            Title = "Barcode1";
            Data = "Data1";
        }

        public DelegateCommand GenerateBarcodeCommand { get; private set; }
        public DelegateCommand AdditionalInputCommand { get; private set; }
        public DelegateCommand OpenInNewWindowCommand { get; private set; }
        public DelegateCommand<BarcodeResultViewModel> CopyToClipboardCommand { get; private set; }
        public DelegateCommand<BarcodeResultViewModel> DeleteCommand { get; private set; }

        private void InitializeBarcodeTypes()
        {
            BarcodeTypes.AddRange(new ObservableCollection<BarcodeTypeViewModel>
            {
                new BarcodeTypeViewModel { Type = BarcodeType.Ean13 },
                new BarcodeTypeViewModel { Type = BarcodeType.Ean128 },
                new BarcodeTypeViewModel { Type = BarcodeType.Code128 },
                new BarcodeTypeViewModel
                {
                    Type = BarcodeType.DataMatrix,
                    AdditionalInput = appWindowsService.OpenNmvsInputWindow
                },
                new BarcodeTypeViewModel { Type = BarcodeType.QRCode },
            });
            SelectedBarcodeType = BarcodeTypes.First();
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

        public void GenerateBarcode()
        {
            if (!GenerateValidation())
                return;

            var barcodeData = new BarcodeData
            {
                Data = Data,
                Type = SelectedBarcodeType.Type
            };

            try
            {
                GenerateBarcode(barcodeData, Title);
                StatusMessage = $"Barcode \"{Title}\" generate successfully!";
                RaisePropertyChanged(nameof(BarcodesCount));
            }
            catch (Exception exc)
            {
                dialogsService.ShowException("Exception during barcode generation", exc);
            }
        }

        public void GenerateBarcode(BarcodeData barcodeData, string title)
        {
            var barcode = barcodesGenerator.CreateBarcode(barcodeData);
            Barcodes.Insert(0, new BarcodeResultViewModel
            {
                Barcode = barcode,
                Data = barcodeData.Data,
                Title = title,
                Type = barcodeData.Type,
            });
            RaisePropertyChanged(nameof(BarcodesCount));
        }

        public void Delete(BarcodeResultViewModel barcode)
        {
            if (barcode == null)
                return;

            if (!dialogsService.ShowYesNoQuestion($"Do you really want to delete barcode \"{barcode.Title}?\""))
                return;

            Barcodes.Remove(barcode);
            StatusMessage = $"Successfully removed \"{barcode.Title}\"";
            RaisePropertyChanged(nameof(BarcodesCount));
        }

        public void AdditionalInput()
        {
            var result = SelectedBarcodeType.AdditionalInput();
            if (string.IsNullOrEmpty(result))
                return;

            Data = result;
        }

        private void OpenInNewWindow()
        {
            if (SelectedBarcode == null)
                return;

            appWindowsService.OpenBarcodeWindow(SelectedBarcode);
        }

        private void CopyToClipboard(BarcodeResultViewModel barcode)
        {
            if (barcode == null)
                return;

            systemService.CopyToClipboard(barcode.Barcode);
            StatusMessage = $"Barcode \"{barcode.Title}\" copied to clipboard";
        }
    }
}
