using Barcodes.Utils;
using System.Collections.Generic;

namespace Barcodes.Services.Storage
{
    public class StorageService : IStorageService
    {
        private Storage loadedStorage = new Storage();

        public Storage Load(string filePath, bool throwException = false)
        {
            try
            {
                loadedStorage = Serializer.FromFile<Storage>(filePath);
                return loadedStorage;
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

        public void Save(string filePath, Storage storage)
        {
            Serializer.ToFile(storage, filePath);
        }

        public bool StorageChanged(Storage currentStorage)
        {
            return !ObjectUtils.ObjectEquality(loadedStorage, currentStorage);
        }
    }
}
