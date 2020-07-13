using Barcodes.Core.Models;
using Barcodes.Services.Dialogs;
using System.Collections.Generic;

namespace Barcodes.Core.Abstraction
{
    public interface IAppDialogsService : IDialogsService
    {
        string OpenStorageFile(string currentFilePath = null, bool ensureFileExists = true);

        string SaveStorageFile(string currentFilePath);

        string ImportWorkspaceFile();

        List<string> ImportWorkspaceFiles();

        string ExportWorkspaceFile(string workspaceName);

        string ImportBarcodesFile();

        List<string> ImportBarcodesFiles();

        string ExportBarcodesFile();

        string SavePdfFile();

        string SavePngFile(string defaultFileName);

        PdfExportMode ShowPdfExportQuestion();

        SavingMode ShowSavingQuestion();
    }
}