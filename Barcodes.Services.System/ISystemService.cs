using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Barcodes.Services.System
{
    public interface ISystemService
    {
        void CopyToClipboard(BitmapSource bitmapSource);
        void OpenLocation(string location);
        void StartProcess(string process, string arguments = null);
    }
}
