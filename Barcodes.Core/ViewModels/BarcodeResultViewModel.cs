using Barcodes.Services.Generator;
using Prism.Mvvm;
using System.Windows.Media.Imaging;

namespace Barcodes.Core.ViewModels
{
    public class BarcodeResultViewModel : BindableBase
    {
        private string title;
        private BitmapSource barcode;

        public BarcodeResultViewModel(GenerationData generationData)
        {
            GenerationData = generationData;
        }

        public GenerationData GenerationData { get; }

        public string HeaderTitle
        {
            get { return $"Barcodes - {Title}"; }
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public string TypeTitle
        {
            get => GenerationData.Type.ToString();
        }

        public BarcodeType Type
        {
            get => GenerationData.Type;
        }

        public string Data
        {
            get => GenerationData.Data;
        }

        public BitmapSource Barcode
        {
            get => barcode;
            set => SetProperty(ref barcode, value);
        }

        public int BarcodeBitmapWidth
        {
            get => Barcode.PixelWidth;
        }

        public int BarcodeBitmapHeight
        {
            get => Barcode.PixelHeight;
        }

        public int BarcodeWidth
        {
            get => GenerationData.Width;
        }

        public int BarcodeHeight
        {
            get => GenerationData.Height;
        }
    }
}
