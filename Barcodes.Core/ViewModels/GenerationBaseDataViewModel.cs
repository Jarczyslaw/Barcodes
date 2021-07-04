using Barcodes.Codes;
using Barcodes.Services.AppSettings;
using Barcodes.Services.Generator;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;

namespace Barcodes.Core.ViewModels
{
    public class GenerationBaseDataViewModel : BindableBase
    {
        private bool defaultSize = true;
        private int width = 100;
        private int height = 100;
        private bool validateCodeText = true;

        private BarcodeTypeViewModel selectedType;
        private ObservableCollection<BarcodeTypeViewModel> types;

        public GenerationBaseDataViewModel()
        {
            InitializeBarcodeTypes();
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
                new BarcodeTypeViewModel(BarcodeType.EAN13),
                new BarcodeTypeViewModel(BarcodeType.ITF14),
                new BarcodeTypeViewModel(BarcodeType.EAN128),
                new BarcodeTypeViewModel(BarcodeType.Code128),
                new BarcodeTypeViewModel(BarcodeType.DataMatrix),
                new BarcodeTypeViewModel(BarcodeType.QRCode),
            };
        }

        public void SelectType(BarcodeType barcodeType)
        {
            SelectedType = Types.FirstOrDefault(b => b.Type == barcodeType);
        }

        public virtual GenerationData ToData()
        {
            return new GenerationData
            {
                Type = SelectedType.Type,
                DefaultSize = DefaultSize,
                Width = Width,
                Height = Height,
                ValidateCodeText = ValidateCodeText
            };
        }

        public virtual void FromData(GenerationData generationData)
        {
            SelectType(generationData.Type);
            DefaultSize = generationData.DefaultSize;
            Width = generationData.Width;
            Height = generationData.Height;
            ValidateCodeText = generationData.ValidateCodeText;
        }

        public virtual void FromSettings(GenerationSettings generationSettings)
        {
            SelectType(generationSettings.Type);
            DefaultSize = generationSettings.DefaultSize;
            Height = generationSettings.Height;
            Width = generationSettings.Width;
            ValidateCodeText = generationSettings.ValidateCodeText;
        }

        public virtual GenerationSettings ToSettings()
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