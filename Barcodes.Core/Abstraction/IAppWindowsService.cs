using Barcodes.Codes;
using Barcodes.Core.Models;
using Barcodes.Core.ViewModels;
using Barcodes.Services.AppSettings;
using System;
using System.Collections.Generic;

namespace Barcodes.Core.Abstraction
{
    public interface IAppWindowsService
    {
        IntPtr MainWindowHandle { get; }
        int WindowsCount { get; }

        void CloseShell();

        void CloseBarcodesWindows();

        void CloseWorkspacesWindows();

        void CloseQuickGeneratorsWindows();

        void CloseAllWindows();

        void OpenBarcodeWindow(BarcodeViewModel barcodeViewModel);

        void OpenWorkspaceWindow(WorkspaceViewModel workspaceViewModel);

        TemplateResult OpenTemplateWindow<TViewModel>(object parentViewModel, string data);

        void ShowAboutWindow(object parentViewModel, AboutViewModel aboutViewModel);

        GenerationResult ShowExamplesWindow(object parentViewModel, ExamplesViewModel examplesViewModel);

        GenerationResult ShowGenerationWindow(object parentViewModel, BarcodeViewModel barcode, bool edit, BarcodeTemplate? template = null);

        string ShowWorkspaceNameWindow(string currentName, Func<string, bool> validationRule);

        WorkspaceViewModel SelectBarcodesWorkspace(IEnumerable<WorkspaceViewModel> workspaces);

        SettingsSaveResult ShowSettingsWindow(object parentViewModel);

        AppSettings ShowRawSettingsWindow(object parentViewModel, AppSettings appSettings);

        void ShowStorageWindow(AppViewModel appViewModel, List<WorkspaceViewModel> workspaces);

        void ShowQuickGeneratorWindow(AppViewModel appViewModel);

        StorageBarcodeViewModel SelectStorageBarcode(object parentViewModel, List<StorageBarcodeViewModel> barcodes);

        string ShowBarcodeTitleWindow(object parentViewModel, Func<string, bool> validationRule);

        void ShowMainAppWindow();
    }
}