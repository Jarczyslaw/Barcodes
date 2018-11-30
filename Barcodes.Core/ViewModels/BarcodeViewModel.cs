using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Barcodes.Core.ViewModels
{
    public class BarcodeViewModel : BindableBase
    {
        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private string typeTitle;
        public string TypeTitle
        {
            get => typeTitle;
            set => SetProperty(ref typeTitle, value);
        }

        private string data;
        public string Data
        {
            get => data;
            set => SetProperty(ref data, value);
        }

        private BitmapSource barcode;
        public BitmapSource Barcode
        {
            get => barcode;
            set => SetProperty(ref barcode, value);
        }
    }
}
