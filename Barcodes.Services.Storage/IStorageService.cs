using System.Collections.Generic;

namespace Barcodes.Services.Storage
{
    public interface IStorageService
    {
        Storage Load(string filePath, bool throwException = false);
        void Save(string filePath, Storage storage);
        bool StorageChanged(Storage currentStorage);
    }
}
