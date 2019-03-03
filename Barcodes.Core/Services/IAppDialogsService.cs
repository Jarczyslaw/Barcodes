using Barcodes.Services.Dialogs;

namespace Barcodes.Core.Services
{
    public interface IAppDialogsService : IDialogsService
    {
        string OpenStorageFile(string currentFilePath);
        string SaveStorageFile(string currentFilePath);
        string ImportWorkspaceFile();
        string ExportWorkspaceFile(string workspaceName);
        string ImportBarcodeFile();
        string ExportBarcodeFile(string barcodeTitle);
        string SavePdfFile();
        string SavePngFile(string defaultFileName);
        PdfExportMode ShowPdfExportQuestion();
    }
}
