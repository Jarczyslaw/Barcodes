﻿using Barcodes.Core.ViewModels;
using Barcodes.Services.Generator;

namespace Barcodes.Core.Services
{
    public interface IAppWindowsService
    {
        void OpenBarcodeWindow(object barcodeViewModel);
        string OpenNmvsProductWindow();
        string OpenEan128ProductWindow();
        void ShowHelpWindow();
        GenerationViewModel.GenerationViewModelResult ShowGenerationWindow(BarcodeResultViewModel barcode = null);
    }
}
