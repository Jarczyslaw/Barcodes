﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Core.Services
{
    public interface IBarcodeWindowsService
    {
        void OpenBarcodeWindow(object barcodeViewModel);
        string OpenNmvsInputWindow();
    }
}
