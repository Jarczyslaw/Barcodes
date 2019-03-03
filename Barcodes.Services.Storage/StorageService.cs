﻿using Barcodes.Utils;

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

        public StorageWorkspace ImportWorkspace(string filePath)
        {
            return Serializer.FromFile<StorageWorkspace>(filePath);
        }

        public void ExportWorkspace(string filePath, StorageWorkspace workspace)
        {
            Serializer.ToFile(workspace, filePath);
        }

        public StorageBarcode ImportBarcode(string filePath)
        {
            return Serializer.FromFile<StorageBarcode>(filePath);
        }

        public void ExportBarcode(string filePath, StorageBarcode barcode)
        {
            Serializer.ToFile(barcode, filePath);
        }

        public bool StorageChanged(Storage currentStorage)
        {
            return !ObjectUtils.ObjectEquality(loadedStorage, currentStorage);
        }
    }
}
