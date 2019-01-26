using System.Windows.Media.Imaging;

namespace Barcodes.Services.Generator
{
    public interface IGeneratorService
    {
        BitmapSource CreateQRBarcode(int size, string textData);
        BitmapSource CreateBarcode(GenerationData barcodeData);
    }
}
