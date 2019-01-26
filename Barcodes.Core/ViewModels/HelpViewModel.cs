using Barcodes.Services.Generator;
using Barcodes.Utils;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Media.Imaging;

namespace Barcodes.Core.ViewModels
{
    public class HelpViewModel : BindableBase
    {
        private BitmapSource randomBarcode;

        private readonly IGeneratorService barcodesGenerator;

        public HelpViewModel(IGeneratorService barcodesGenerator)
        {
            this.barcodesGenerator = barcodesGenerator;

            GenerateRandomBarcodeCommand = new DelegateCommand(GenerateRandomBarcode);
            GenerateRandomBarcode();
        }

        public DelegateCommand GenerateRandomBarcodeCommand { get; }

        public BitmapSource RandomBarcode
        {
            get => randomBarcode;
            set => SetProperty(ref randomBarcode, value);
        }

        public void GenerateRandomBarcode()
        {
            var randomText = RandomTexts.Get();
            RandomBarcode = barcodesGenerator.CreateQRBarcode(300, randomText);
        }
    }
}
