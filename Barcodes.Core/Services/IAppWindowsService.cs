﻿using Barcodes.Core.ViewModels;
using Barcodes.Core.ViewModelsResult;
using System;
using System.Collections.Generic;

namespace Barcodes.Core.Services
{
    public interface IAppWindowsService
    {
        int WindowsCount { get; }

        void CloseBarcodesAndWorkspacesWindows();

        void OpenBarcodeWindow(BarcodeViewModel barcodeViewModel);

        void OpenWorkspaceWindow(WorkspaceViewModel workspaceViewModel);

        string OpenNmvsProductWindow(string nmvsData);

        string OpenEan128ProductWindow(string ean128Data);

        void ShowAboutWindow();

        BarcodeViewModel ShowExamplesWindow();

        GenerationViewModelResult ShowGenerationWindow(BarcodeViewModel barcode, bool edit);

        string ShowWorkspaceNameWindow(string currentName, Func<string, bool> validationRule);

        WorkspaceViewModel SelectBarcodesWorkspace(IEnumerable<WorkspaceViewModel> workspaces);

        bool ShowSettingsWindow();
    }
}