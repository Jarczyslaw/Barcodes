using Barcodes.Services.Generator;
using Barcodes.Utils;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Barcodes.Core.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private readonly IGeneratorService generator;
        private ImageSource barcode;

        public AboutViewModel(IGeneratorService generator)
        {
            this.generator = generator;
        }

        public string VersionText
        {
            get
            {
                var appVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
                return $"Version: {appVersion}";
            }
        }

        public ImageSource Barcode
        {
            get => barcode;
            set => SetProperty(ref barcode, value);
        }

        public void GenerateRandomBarcode()
        {
            var randomText = RandomTexts.Get();
            Barcode = generator.CreateQRBarcode(300, randomText);
        }

        public Task GenerateRandomBarcodeAsync()
        {
            return HeavyAction("Generating barcode...", () => GenerateRandomBarcode());
        }
    }
}