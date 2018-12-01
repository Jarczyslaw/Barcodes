using Barcodes.Core.Views;
using Barcodes.Services.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Core.Services
{
    public class BarcodeWindowsService : WindowsService, IBarcodeWindowsService
    {
        public void OpenBarcodeWindow(object barcodeViewModel)
        {
            Show<BarcodeWindow>(barcodeViewModel);
        }
    }
}
