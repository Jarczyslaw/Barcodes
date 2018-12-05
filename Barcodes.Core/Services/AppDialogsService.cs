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
        private readonly DialogFilterPair defaultFilter = new DialogFilterPair { DisplayName = "json", ExtensionsList = "json" };

        public string OpenStorageFilePath(string currentFilePath)
        {
            var directoryPath = Path.GetDirectoryName(currentFilePath);
            return OpenFile("Barcodes storage file", directoryPath, defaultFilter);
        }

        public string SaveStorageFilePath(string currentFilePath)
        {
            var fileName = Path.GetFileName(currentFilePath);
            var directoryPath = Path.GetDirectoryName(currentFilePath);
            return SaveFile("Barcodes storage file", directoryPath, fileName, defaultFilter);
        }
    }
}
