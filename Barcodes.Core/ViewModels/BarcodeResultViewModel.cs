using Barcodes.Services.Generator;
using Prism.Mvvm;
using System.Windows.Media.Imaging;

namespace Barcodes.Core.ViewModels
{
    public class BarcodeResultViewModel : BindableBase
    {
        private string title;
        private string data;
        private BitmapSource barcode;

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
            get => Type.ToString();
        }

        public BarcodeType Type { get; set; }

        public string Data
        {
            get => data;
            set => SetProperty(ref data, value);
        }

        public int BarcodeBitmapWidth
        {
            get => Barcode.PixelWidth;
        }

        public int BarcodeBitmapHeight
        {
            get => Barcode.PixelHeight;
        }

        public int BarcodeWidth { get; set; }

        public int BarcodeHeight { get; set; }

        public BitmapSource Barcode
        {
            get => barcode;
            set => SetProperty(ref barcode, value);
        }
    }
}
