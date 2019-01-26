using Barcodes.Utils;
using System.Collections.Generic;

namespace Barcodes.Services.Storage
{
    public class StorageService : IStorageService
    {
        public List<StorageEntry> Load(string filePath, bool throwException = false)
        {
            try
            {
                return Serializer.FromFile<List<StorageEntry>>(filePath);
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

        public void Save(string filePath, List<StorageEntry> barcodes)
        {
            Serializer.ToFile(barcodes, filePath);
        }
    }
}
