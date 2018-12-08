using Barcodes.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Core.Services
{
    public class AppDialogsService : DialogsService, IAppDialogsService
    {
        private readonly DialogFilterPair jsonFilter = new DialogFilterPair { DisplayName = "json", ExtensionsList = "json" };
        private readonly DialogFilterPair pdfFilter = new DialogFilterPair { DisplayName = "pdf", ExtensionsList = "pdf" };

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
            return SaveFile("Barcodes PDF file", null, "barcodes.pdf", pdfFilter);
        }
    }
}
