using Barcodes.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Core.Services
{
    public interface IAppDialogsService : IDialogsService
    {
        string OpenStorageFile(string currentFilePath);
        string SaveStorageFile(string currentFilePath);
        string SavePdfFile();
    }
}
