using JToolbox.Desktop.Core;
using System.Drawing;
using System.Windows;

namespace JToolbox.WPF.Core.Extensions
{
    public static class ScreenCaptureExtensions
    {
        public static Bitmap CapturePrimaryScreen(this ScreenCapture screenCapture)
        {
            return screenCapture.Capture(new Rectangle(0, 0, 
                (int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight));
        }

        public static Bitmap CaptureAllScreens(this ScreenCapture screenCapture)
        {
            return screenCapture.Capture(new Rectangle((int)SystemParameters.VirtualScreenLeft, (int)SystemParameters.VirtualScreenTop,
                (int)SystemParameters.VirtualScreenWidth, (int)SystemParameters.VirtualScreenHeight));
        }
    }
}