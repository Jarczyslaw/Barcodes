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
        public BitmapSource RandomBarcode
        {
            get => randomBarcode;
            set => SetProperty(ref randomBarcode, value);
        }

        private readonly IBarcodesGeneratorService barcodesGenerator;

        public HelpViewModel(IBarcodesGeneratorService barcodesGenerator)
        {
            this.barcodesGenerator = barcodesGenerator;

            GenerateRandomBarcodeCommand = new DelegateCommand(GenerateRandomBarcode);
            GenerateRandomBarcode();
        }

        public DelegateCommand GenerateRandomBarcodeCommand { get; }

        public void GenerateRandomBarcode()
        {
            var randomText = RandomTexts.Get();
            RandomBarcode = barcodesGenerator.CreateShellBarcode(300, randomText);
        }
    }
}
