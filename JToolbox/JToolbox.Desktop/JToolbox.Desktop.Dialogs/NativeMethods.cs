using System;
using System.Runtime.InteropServices;

namespace JToolbox.Desktop.Dialogs
{
    internal static class NativeMethods
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetActiveWindow();
    }
}
