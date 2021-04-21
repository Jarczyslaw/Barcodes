using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace JToolbox.Desktop.Core
{
    public class ScreenCapture
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);

        public Bitmap CaptureForegroundWindow()
        {
            return Capture(GetBoundFromWindow(GetForegroundWindow()));
        }

        public Bitmap CaptureWindow(Rectangle windowBounds)
        {
            return Capture(windowBounds);
        }

        public Bitmap CaptureProcess(Process process)
        {
            return CaptureWindow(GetBoundFromWindow(process.MainWindowHandle));
        }

        public Bitmap Capture(Rectangle bounds)
        {
            var bitmap = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size, CopyPixelOperation.SourceCopy);
            }
            return bitmap;
        }

        private Rectangle GetBoundFromWindow(IntPtr intPtr)
        {
            var rect = new Rect();
            GetWindowRect(intPtr, ref rect);
            return new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
        }
    }
}