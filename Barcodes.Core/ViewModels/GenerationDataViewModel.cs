using Barcodes.Codes;
using Barcodes.Services.AppSettings;
using Barcodes.Services.Generator;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;

namespace Barcodes.Core.ViewModels
{
    public class GenerationDataViewModel : BindableBase
    {
        private string data = "Barcode Data";
        private bool defaultSize = true;
        private int width = 100;
        private int height = 100;
        private bool validateCodeText = true;

        private BarcodeTypeViewModel selectedType;
        private ObservableCollection<BarcodeTypeViewModel> types;

        public GenerationDataViewModel()
        {
            InitializeBarcodeTypes();
        }

        public string Data
        {
            get => data;
            set => SetProperty(ref data, value);
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

        public BarcodeTypeViewModel SelectedType
        {
            get => selectedType;
            set => SetProperty(ref selectedType, value);
        }

        public ObservableCollection<BarcodeTypeViewModel> Types
        {
            get => types;
            set => SetProperty(ref types, value);
        }

        private void InitializeBarcodeTypes()
        {
            Types = new ObservableCollection<BarcodeTypeViewModel>()
            {
                new BarcodeTypeViewModel(BarcodeType.Ean13),
                new BarcodeTypeViewModel(BarcodeType.Ean128),
                new BarcodeTypeViewModel(BarcodeType.Code128),
                new BarcodeTypeViewModel(BarcodeType.DataMatrix),
                new BarcodeTypeViewModel(BarcodeType.QRCode),
            };
        }

        public void SelectType(BarcodeType barcodeType)
        {
            SelectedType = Types.FirstOrDefault(b => b.Type == barcodeType);
        }

        public GenerationData ToData()
        {
            return new GenerationData
            {
                Data = Data.Trim(),
                Type = SelectedType.Type,
                DefaultSize = DefaultSize,
                Width = Width,
                Height = Height,
                ValidateCodeText = ValidateCodeText
            };
        }

        public void FromData(GenerationData generationData)
        {
            Data = generationData.Data;
            SelectType(generationData.Type);
            DefaultSize = generationData.DefaultSize;
            Width = generationData.Width;
            Height = generationData.Height;
            ValidateCodeText = generationData.ValidateCodeText;
        }

        public void FromSettings(GenerationSettings generationSettings)
        {
            SelectType(generationSettings.Type);
            DefaultSize = generationSettings.DefaultSize;
            Height = generationSettings.Height;
            Width = generationSettings.Width;
            ValidateCodeText = generationSettings.ValidateCodeText;
        }

        public GenerationSettings ToSettings()
        {
            return new GenerationSettings
            {
                Width = Width,
                DefaultSize = DefaultSize,
                Height = Height,
                Type = SelectedType.Type,
                ValidateCodeText = ValidateCodeText
            };
        }
    }
}