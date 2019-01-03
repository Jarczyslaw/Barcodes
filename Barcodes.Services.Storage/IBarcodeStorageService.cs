using System.Collections.Generic;

namespace Barcodes.Services.Storage
{
    public interface IBarcodeStorageService
    {
        List<BarcodeStorageEntry> Load(string filePath, bool throwException = false);
        void Save(string filePath, List<BarcodeStorageEntry> barcodes);
    }
}
