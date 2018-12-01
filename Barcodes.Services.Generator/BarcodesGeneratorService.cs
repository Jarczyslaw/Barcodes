using Aspose.BarCode.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Barcodes.Utils;

namespace Barcodes.Services.Generator
{
    public class BarcodesGeneratorService : IBarcodesGeneratorService
    {
        public BitmapSource CreateShellBarcode(int size, string textData)
        {
            using (var generator = new BarCodeGenerator(EncodeTypes.QR, textData))
            {
                generator.AutoSizeMode = AutoSizeMode.Nearest;
                generator.BarCodeWidth.Pixels = size;
                generator.BarCodeHeight.Pixels = size;
                generator.D2.DisplayText = string.Empty;
                return generator.GenerateBarCodeImage().ToBitmapSource();
            }
        }

        public BitmapSource CreateBarcode(BarcodeData barcodeData)
        {
            using (var generator = new BarCodeGenerator(barcodeData.Type, barcodeData.Data))
            {
                generator.ThrowExceptionWhenCodeTextIncorrect = true;
                generator.AutoSizeMode = AutoSizeMode.Nearest;
                if (!barcodeData.ShowData)
                {
                    generator.D2.DisplayText = string.Empty;
                    generator.CaptionAbove.Visible =
                        generator.CaptionBelow.Visible = false;
                }
                generator.RecalculateValues();
                if (barcodeData.MinWidth.HasValue && generator.BarCodeWidth.Pixels < barcodeData.MinWidth.Value)
                {
                    generator.BarCodeWidth.Pixels = barcodeData.MinWidth.Value;
                    if (barcodeData.Type.Classification == BarcodeClassifications.Type2D)
                        generator.BarCodeHeight.Pixels = generator.BarCodeWidth.Pixels;
                }  
                var barcodeImage = generator.GenerateBarCodeImage();
                return barcodeImage.ToBitmapSource();
            }
        }
    }
}
