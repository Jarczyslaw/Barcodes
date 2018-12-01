using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Barcodes.Services.Generator
{
    public interface IBarcodesGeneratorService
    {
        BitmapSource CreateShellBarcode(int size, string textData);
        BitmapSource CreateBarcode(BarcodeData barcodeData);
    }
}
