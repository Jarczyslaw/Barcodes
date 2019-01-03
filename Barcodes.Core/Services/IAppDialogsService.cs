using Barcodes.Services.Dialogs;

namespace Barcodes.Core.Services
{
    public interface IAppDialogsService : IDialogsService
    {
        string OpenStorageFile(string currentFilePath);
        string SaveStorageFile(string currentFilePath);
        string SavePdfFile();
    }
}
