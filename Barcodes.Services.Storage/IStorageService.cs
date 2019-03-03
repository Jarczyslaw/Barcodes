namespace Barcodes.Services.Storage
{
    public interface IStorageService
    {
        Storage Load(string filePath, bool throwException = false);
        void Save(string filePath, Storage storage);
        StorageWorkspace ImportWorkspace(string filePath);
        void ExportWorkspace(string filePath, StorageWorkspace workspace);
        StorageBarcode ImportBarcode(string filePath);
        void ExportBarcode(string filePath, StorageBarcode barcode);
        bool StorageChanged(Storage currentStorage);
    }
}
