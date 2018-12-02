using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Services.Storage
{
    public interface IBarcodeStorageService
    {
        List<BarcodeStorageEntry> Load(string filePath, bool throwException = false);
        void Save(string filePath, List<BarcodeStorageEntry> barcodes);
    }
}
