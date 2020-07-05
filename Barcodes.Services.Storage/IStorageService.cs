using System.Collections.Generic;

namespace Barcodes.Services.Storage
{
    public interface IStorageService
    {
        Storage Load(string filePath, bool throwException = false);
        void Save(string filePath, Storage storage);
        StorageWorkspace ImportWorkspace(string filePath);
        void ExportWorkspace(string filePath, StorageWorkspace workspace);
        List<StorageBarcode> ImportBarcodes(string filePath);
        void ExportBarcodes(string filePath, List<StorageBarcode> barcode);
        bool StorageChanged(Storage currentStorage);
    }
}
