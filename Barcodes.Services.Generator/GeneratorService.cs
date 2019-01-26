using Aspose.BarCode.Generation;
using Barcodes.Utils;
using System.Windows.Media.Imaging;

namespace Barcodes.Services.Generator
{
    public class GeneratorService : IGeneratorService
    {
        public BitmapSource CreateQRBarcode(int size, string textData)
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

        public BitmapSource CreateBarcode(GenerationData barcodeData)
        {
            var encodeType = barcodeData.Type.GetEncodeType();
            using (var generator = new BarCodeGenerator(encodeType, barcodeData.Data))
            {
                generator.ThrowExceptionWhenCodeTextIncorrect = barcodeData.ValidateCodeText;
                generator.AutoSizeMode = AutoSizeMode.Nearest;
                generator.EnableEscape = true;
                HideTexts(generator);
                FitSizes(generator, barcodeData);
                var barcodeImage = generator.GenerateBarCodeImage();
                return barcodeImage.ToBitmapSource();
            }
        }

        private void FitSizes(BarCodeGenerator generator, GenerationData barcodeData)
        {
            generator.RecalculateValues();
            if (barcodeData.DefaultSize && generator.BarCodeWidth.Pixels < barcodeData.MinWidth)
            {
                generator.BarCodeWidth.Pixels = barcodeData.MinWidth;
                if (generator.EncodeType.Classification == BarcodeClassifications.Type2D)
                {
                    generator.BarCodeHeight.Pixels = generator.BarCodeWidth.Pixels;
                }
            }
            else if (!barcodeData.DefaultSize)
            {
                generator.BarCodeWidth.Pixels = barcodeData.Width;
                generator.BarCodeHeight.Pixels = barcodeData.Height;
            }
        }

        private void HideTexts(BarCodeGenerator generator)
        {
            generator.D2.DisplayText = string.Empty;
            generator.CaptionAbove.Visible =
                generator.CaptionBelow.Visible = false;
        }
    }
}
