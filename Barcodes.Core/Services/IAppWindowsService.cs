﻿using Barcodes.Core.ViewModels;
using Barcodes.Core.ViewModelsResult;
using Barcodes.Services.Generator;
using System;
using System.Collections.Generic;

namespace Barcodes.Core.Services
{
    public interface IAppWindowsService
    {
        void OpenBarcodeWindow(object barcodeViewModel);
        string OpenNmvsProductWindow(string nmvsData);
        string OpenEan128ProductWindow(string ean128Data);
        void ShowHelpWindow();
        GenerationViewModelResult ShowGenerationWindow(BarcodeViewModel barcode = null);
        string ShowWorkspaceNameWindow(string currentName, Func<string, bool> validationRule);
        WorkspaceViewModel SelectBarcodesWorkspace(IEnumerable<WorkspaceViewModel> workspaces);
    }
}
