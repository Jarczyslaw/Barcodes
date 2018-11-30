﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Barcodes.Services.Generator
{
    public interface IBarcodesGeneratorService
    {
        BitmapSource CreateRandomBarcode(int size);
        BitmapSource CreateBarcode(BarcodeData barcodeData);
    }
}
