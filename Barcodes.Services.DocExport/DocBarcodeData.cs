using System.Windows.Media.Imaging;

namespace Barcodes.Services.DocExport
{
    public class DocBarcodeData
    {
        public string Title { get; set; }
        public string Data { get; set; }
        public BitmapSource Barcode { get; set; }
        public string TempFilePath { get; set; }
    }
}
