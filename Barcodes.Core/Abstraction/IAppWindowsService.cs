using Barcodes.Codes;
using Barcodes.Core.Models;
using Barcodes.Core.ViewModels;
using System;
using System.Collections.Generic;

namespace Barcodes.Core.Abstraction
{
    public interface IAppWindowsService
    {
        int WindowsCount { get; }

        void CloseBarcodesAndWorkspacesWindows();

        void OpenBarcodeWindow(BarcodeViewModel barcodeViewModel);

        void OpenWorkspaceWindow(WorkspaceViewModel workspaceViewModel);

        TemplateResult OpenTemplateWindow<TViewModel>(string data);

        void ShowAboutWindow();

        BarcodeViewModel ShowExamplesWindow();

        GenerationResult ShowGenerationWindow(BarcodeViewModel barcode, bool edit, BarcodeTemplate? template = null);

        string ShowWorkspaceNameWindow(string currentName, Func<string, bool> validationRule);

        WorkspaceViewModel SelectBarcodesWorkspace(IEnumerable<WorkspaceViewModel> workspaces);

        bool ShowSettingsWindow();
    }
}