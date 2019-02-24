using Barcodes.Utils;
using System.Collections.Generic;

namespace Barcodes.Services.Storage
{
    public class StorageService : IStorageService
    {
        public List<StorageWorkspace> Load(string filePath, bool throwException = false)
        {
            try
            {
                return Serializer.FromFile<List<StorageWorkspace>>(filePath);
            }
            catch
            {
                if (throwException)
                {
                    throw;
                }

                return null;
            }
        }

        public void Save(string filePath, List<StorageWorkspace> workspaces)
        {
            Serializer.ToFile(workspaces, filePath);
        }
    }
}
