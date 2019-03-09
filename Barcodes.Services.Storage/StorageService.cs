using Barcodes.Utils;
using System.IO;

namespace Barcodes.Services.Storage
{
    public class StorageService : IStorageService
    {
        private Storage currentStorage = new Storage();

        public Storage Load(string filePath, bool throwException = false)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            currentStorage = Serializer.FromFile<Storage>(filePath);
            return currentStorage;
        }

        public void Save(string filePath, Storage storage)
        {
            Serializer.ToFile(storage, filePath);
            currentStorage = storage;
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
            return !ObjectUtils.ObjectEquality(this.currentStorage, currentStorage);
        }
    }
}
