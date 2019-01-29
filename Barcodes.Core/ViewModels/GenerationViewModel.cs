using Barcodes.Core.Events;
using Barcodes.Core.Services;
using Barcodes.Core.Services.StateSaver.States;
using Barcodes.Services.Generator;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Barcodes.Core.ViewModels
{
    public class GenerationViewModel : BindableBase
    {
        private string data = "Barcode Data";
        private string title = "Barcode Title";
        private bool defaultSize = true;
        private int width = 100;
        private int height = 100;
        private bool validateCodeText = true;

        private BarcodeTypeViewModel selectedBarcodeType;
        private ObservableCollection<BarcodeTypeViewModel> barcodeTypes;
        private AdditionalInputViewModel selectedAdditionalInput;
        private ObservableCollection<AdditionalInputViewModel> additionalInputs;

        private readonly IServicesContainer services;

        public GenerationViewModel(IServicesContainer services)
        {
            this.services = services;

            InitializeBarcodeTypes();
            InitializeAdditionalInputs();

            AcceptCommand = new DelegateCommand(GenerateBarcode);
            CancelCommand = new DelegateCommand(() => services.EventAggregator.GetEvent<GenerationWindowClose>().Publish());
            AdditionalInputCommand = new DelegateCommand(AdditionalInput, () => AdditionalInputEnabled);

            services.StateSaverService.Load<GenerationViewModel, GenerationViewModelState>(this);
        }

        public DelegateCommand AcceptCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand AdditionalInputCommand { get; }

        public BarcodeResultViewModel Result { get; private set; }

        public string Data
        {
            get => data;
            set => SetProperty(ref data, value);
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public bool DefaultSize
        {
            get => defaultSize;
            set => SetProperty(ref defaultSize, value);
        }

        public int Width
        {
            get => width;
            set => SetProperty(ref width, value);
        }

        public int Height
        {
            get => height;
            set => SetProperty(ref height, value);
        }

        public bool ValidateCodeText
        {
            get => validateCodeText;
            set => SetProperty(ref validateCodeText, value);
        }

        public BarcodeTypeViewModel SelectedBarcodeType
        {
            get => selectedBarcodeType;
            set
            {
                SetProperty(ref selectedBarcodeType, value);
                AdditionalInputCommand?.RaiseCanExecuteChanged();
            }
        }

        public ObservableCollection<BarcodeTypeViewModel> BarcodeTypes
        {
            get => barcodeTypes;
            set => SetProperty(ref barcodeTypes, value);
        }

        public AdditionalInputViewModel SelectedAdditionalInput
        {
            get => selectedAdditionalInput;
            set
            {
                SetProperty(ref selectedAdditionalInput, value);
                AdditionalInputEnabled = value.Handler != null;
                AdditionalInputCommand?.RaiseCanExecuteChanged();
            }
        }

        public bool AdditionalInputEnabled { get; set; }

        public int SelectedAdditionalInputIndex
        {
            get => AdditionalInputs.IndexOf(SelectedAdditionalInput);
            set
            {
                if (value != -1)
                {
                    SelectedAdditionalInput = AdditionalInputs[value];
                }
            }
        }

        public ObservableCollection<AdditionalInputViewModel> AdditionalInputs
        {
            get => additionalInputs;
            set => SetProperty(ref additionalInputs, value);
        }

        private void InitializeAdditionalInputs()
        {
            AdditionalInputs = new ObservableCollection<AdditionalInputViewModel>
            {
                new AdditionalInputViewModel
                {
                    Title = "Select additional input"
                },
                new AdditionalInputViewModel
                {
                    Title = "EAN128 - Product code",
                    Handler = services.AppWindowsService.OpenEan128ProductWindow
                },
                new AdditionalInputViewModel
                {
                    Title = "DataMatrix - NMVS",
                    Handler = services.AppWindowsService.OpenNmvsProductWindow
                }
            };
            SelectedAdditionalInput = AdditionalInputs.First();
        }

        private void InitializeBarcodeTypes()
        {
            BarcodeTypes = new ObservableCollection<BarcodeTypeViewModel>()
            {
                new BarcodeTypeViewModel(BarcodeType.Ean13),
                new BarcodeTypeViewModel(BarcodeType.Ean128),
                new BarcodeTypeViewModel(BarcodeType.Code128),
                new BarcodeTypeViewModel(BarcodeType.DataMatrix),
                new BarcodeTypeViewModel(BarcodeType.QRCode),
            };
            SelectedBarcodeType = BarcodeTypes.First(t => t.Type == BarcodeType.DataMatrix);
        }

        private bool GenerateValidation()
        {
            if (string.IsNullOrEmpty(Title))
            {
                services.DialogsService.ShowError("Enter barcode's title");
                return false;
            }

            if (string.IsNullOrEmpty(Data))
            {
                services.DialogsService.ShowError("Enter barcode's data");
                return false;
            }

            return true;
        }

        private void GenerateBarcode()
        {
            if (!GenerateValidation())
            {
                return;
            }

            var barcodeData = new GenerationData
            {
                Data = Data,
                Type = SelectedBarcodeType.Type,
                DefaultSize = DefaultSize,
                Width = Width,
                Height = Height,
                ValidateCodeText = ValidateCodeText
            };

            try
            {
                GenerateBarcode(barcodeData, Title);
            }
            catch (Exception exc)
            {
                services.DialogsService.ShowException("Exception during barcode generation. Try disable Data validation", exc);
            }
        }

        private void GenerateBarcode(GenerationData barcodeData, string title)
        {
            var barcodeBitmap = services.BarcodesGenerator.CreateBarcode(barcodeData);
            barcodeBitmap.Freeze();
            Result = new BarcodeResultViewModel(barcodeData)
            {
                Barcode = barcodeBitmap,
                Title = title
            };
            services.StateSaverService.Save<GenerationViewModel, GenerationViewModelState>(this);
            services.EventAggregator.GetEvent<GenerationWindowClose>().Publish();
        }

        private void AdditionalInput()
        {
            if (SelectedAdditionalInput == null)
            {
                return;
            }

            if (SelectedAdditionalInput.Handler == null)
            {
                return;
            }

            var result = SelectedAdditionalInput.Handler();
            if (string.IsNullOrEmpty(result))
            {
                return;
            }

            Data = result;
        }

        public void Load(BarcodeResultViewModel barcode)
        {
            if (barcode == null)
            {
                return;
            }

            Data = barcode.GenerationData.Data;
            SelectedBarcodeType = BarcodeTypes.FirstOrDefault(b => b.Type == barcode.GenerationData.Type);
            Title = barcode.Title;
            DefaultSize = barcode.GenerationData.DefaultSize;
            Width = barcode.GenerationData.Width;
            Height = barcode.GenerationData.Height;
        }
    }
}
