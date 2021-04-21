using System.IO;
using System.Windows.Media.Imaging;

namespace JToolbox.Desktop.Core.Extensions
{
    public static class BitmapSourceExtensions
    {
        public static byte[] ToByteArray(this BitmapSource bitmapSource)
        {
            int width = bitmapSource.PixelWidth;
            int height = bitmapSource.PixelHeight;
            int stride = width * ((bitmapSource.Format.BitsPerPixel + 7) / 8);

            byte[] pixels = new byte[height * stride];
            bitmapSource.CopyPixels(pixels, stride, 0);
            return pixels;
        }

        public static void ToBmp(this BitmapSource bitmapSource, string filePath)
        {
            var encoder = new BmpBitmapEncoder();
            bitmapSource.ToFile(encoder, filePath);
        }

        public static void ToPng(this BitmapSource bitmapSource, string filePath)
        {
            var encoder = new PngBitmapEncoder();
            bitmapSource.ToFile(encoder, filePath);
        }

        public static void ToFile(this BitmapSource bitmapSource, BitmapEncoder encoder, string filePath)
        {
            BitmapFrame frame = BitmapFrame.Create(bitmapSource);
            encoder.Frames.Add(frame);
            using (var stream = File.Create(filePath))
            {
                encoder.Save(stream);
            }
        }
    }
}
