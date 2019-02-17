using Barcodes.Services.Dialogs;
using System.IO;

namespace Barcodes.Core.Services
{
    public class AppDialogsService : DialogsService, IAppDialogsService
    {
        private readonly DialogFilterPair jsonFilter = new DialogFilterPair { DisplayName = "json", ExtensionsList = "json" };

        public string OpenStorageFile(string currentFilePath)
        {
            var directoryPath = Path.GetDirectoryName(currentFilePath);
            return OpenFile("Barcodes storage file", directoryPath, jsonFilter);
        }

        public string SaveStorageFile(string currentFilePath)
        {
            var fileName = Path.GetFileName(currentFilePath);
            var directoryPath = Path.GetDirectoryName(currentFilePath);
            return SaveFile("Barcodes storage file", directoryPath, fileName, jsonFilter);
        }

        public string SavePdfFile()
        {
            var filter = new DialogFilterPair
            {
                DisplayName = "pdf",
                ExtensionsList = "pdf"
            };
            return SaveFile("Barcodes PDF file", null, "barcodes.pdf", filter);
        }

        public string SavePngFile(string defaultFileName)
        {
            var filter = new DialogFilterPair
            {
                DisplayName = "png",
                ExtensionsList = "png"
            };
            return SaveFile("Barcodes image file", null, defaultFileName + ".png", filter);
        }
    }
}
