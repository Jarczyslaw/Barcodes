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
            var data = new BarcodeData
            {
                Width = size,
                Height = size,
                Data = textData,
                ShowData = false,
                Type = EncodeTypes.QR
            };
            return CreateBarcode(data);
        }

        public BitmapSource CreateBarcode(BarcodeData barcodeData)
        {
            using (var generator = new BarCodeGenerator(barcodeData.Type, barcodeData.Data))
            {
                generator.AutoSizeMode = AutoSizeMode.Interpolation;
                generator.BarCodeWidth.Pixels = barcodeData.Width;
                generator.BarCodeHeight.Pixels = barcodeData.Height;
                if (!barcodeData.ShowData)
                {
                    generator.D2.DisplayText = string.Empty;
                    generator.CaptionAbove.Visible =
                        generator.CaptionBelow.Visible = false;
                }
                return generator.GenerateBarCodeImage().ToBitmapSource();
            }
        }
    }
}
