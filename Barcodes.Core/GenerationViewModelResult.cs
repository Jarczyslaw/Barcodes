﻿using Barcodes.Core.ViewModels;

namespace Barcodes.Core
{
    public class GenerationViewModelResult
    {
        public BarcodeResultViewModel Barcode { get; set; }
        public bool AddAsNew { get; set; }
    }
}
