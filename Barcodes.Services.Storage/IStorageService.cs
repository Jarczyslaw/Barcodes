using System.Collections.Generic;

namespace Barcodes.Services.Storage
{
    public interface IStorageService
    {
        List<StorageWorkspace> Load(string filePath, bool throwException = false);
        void Save(string filePath, List<StorageWorkspace> workspaces);
    }
}
