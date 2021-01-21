using Barcodes.Codes;
using Barcodes.Core.Models;
using Barcodes.Core.ViewModels;
using Barcodes.Services.AppSettings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Barcodes.Core.Abstraction
{
    public interface IAppWindowsService
    {
        int WindowsCount { get; }

        void CloseBarcodesWindows();

        void CloseWorkspacesWindows();

        void CloseQuickGeneratorsWindows();

        void CloseAllWindows();

        void OpenBarcodeWindow(BarcodeViewModel barcodeViewModel);

        void OpenWorkspaceWindow(WorkspaceViewModel workspaceViewModel);

        TemplateResult OpenTemplateWindow<TViewModel>(string data);

        Task ShowAboutWindow(Action beforeShow);

        GenerationResult ShowExamplesWindow(ExamplesViewModel examplesViewModel);

        GenerationResult ShowGenerationWindow(BarcodeViewModel barcode, bool edit, BarcodeTemplate? template = null);

        string ShowWorkspaceNameWindow(string currentName, Func<string, bool> validationRule);

        WorkspaceViewModel SelectBarcodesWorkspace(IEnumerable<WorkspaceViewModel> workspaces);

        SettingsSaveResult ShowSettingsWindow();

        AppSettings ShowRawSettingsWindow(AppSettings appSettings);

        void ShowStorageWindow(AppViewModel appViewModel, List<WorkspaceViewModel> workspaces);

        void ShowQuickGeneratorWindow();
    }
}