using Barcodes.Core.ViewModels;
using Barcodes.Core.ViewModelsResult;
using System;
using System.Collections.Generic;

namespace Barcodes.Core.Services
{
    public interface IAppWindowsService
    {
        void OpenBarcodeWindow(BarcodeViewModel barcodeViewModel);

        string OpenNmvsProductWindow(string nmvsData);

        string OpenEan128ProductWindow(string ean128Data);

        void ShowAboutWindow();

        void ShowExamplesWindow();

        GenerationViewModelResult ShowGenerationWindow(BarcodeViewModel barcode = null);

        string ShowWorkspaceNameWindow(string currentName, Func<string, bool> validationRule);

        WorkspaceViewModel SelectBarcodesWorkspace(IEnumerable<WorkspaceViewModel> workspaces);

        bool ShowSettingsWindow();
    }
}