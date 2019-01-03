using System.Windows.Media.Imaging;

namespace Barcodes.Services.Generator
{
    public interface IBarcodesGeneratorService
    {
        BitmapSource CreateShellBarcode(int size, string textData);
        BitmapSource CreateBarcode(BarcodeData barcodeData);
    }
}
