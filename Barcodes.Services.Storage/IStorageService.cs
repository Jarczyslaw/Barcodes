using System.Collections.Generic;

namespace Barcodes.Services.Storage
{
    public interface IStorageService
    {
        List<StorageEntry> Load(string filePath, bool throwException = false);
        void Save(string filePath, List<StorageEntry> barcodes);
    }
}
