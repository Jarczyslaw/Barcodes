using Barcodes.Services.Dialogs;
using System.Collections.Generic;

namespace Barcodes.Core.Services
{
    public interface IAppDialogsService : IDialogsService
    {
        string OpenStorageFile(string currentFilePath);

        string SaveStorageFile(string currentFilePath);

        string ImportWorkspaceFile();

        List<string> ImportWorkspaceFiles();

        string ExportWorkspaceFile(string workspaceName);

        string ImportBarcodeFile();

        List<string> ImportBarcodeFiles();

        string ExportBarcodeFile(string barcodeTitle);

        string SavePdfFile();

        string SavePngFile(string defaultFileName);

        PdfExportMode ShowPdfExportQuestion();

        ClosingMode ShowClosingQuestion();
    }
}