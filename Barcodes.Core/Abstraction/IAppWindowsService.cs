﻿using Barcodes.Codes;
using Barcodes.Core.Models;
using Barcodes.Core.ViewModels;
using Barcodes.Services.AppSettings;
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

        ExampleBarcodeViewModel ShowExamplesWindow();

        GenerationResult ShowGenerationWindow(BarcodeViewModel barcode, bool edit, BarcodeTemplate? template = null);

        string ShowWorkspaceNameWindow(string currentName, Func<string, bool> validationRule);

        WorkspaceViewModel SelectBarcodesWorkspace(IEnumerable<WorkspaceViewModel> workspaces);

        SettingsSaveResult ShowSettingsWindow();

        AppSettings ShowRawSettingsWindow(AppSettings appSettings);

        List<WorkspaceViewModel> ShowStorageWindow(List<WorkspaceViewModel> workspaces);
    }
}