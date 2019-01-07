using System.Windows.Media.Imaging;

namespace Barcodes.Services.Generator
{
    public interface IBarcodesGeneratorService
    {
        BitmapSource CreateQRBarcode(int size, string textData);
        BitmapSource CreateBarcode(BarcodeData barcodeData);
    }
}
