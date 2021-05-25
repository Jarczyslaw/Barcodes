using System.Collections.Generic;

namespace Barcodes.Services.Storage
{
    public interface IStorageService
    {
        Storage Load(string filePath, bool setCurrentStorage);

        void Save(string filePath, Storage storage);

        StorageWorkspace ImportWorkspace(string filePath);

        void ExportWorkspace(string filePath, StorageWorkspace workspace);

        List<StorageBarcode> ImportBarcodes(string filePath);

        void ExportBarcodes(string filePath, List<StorageBarcode> barcode);

        bool StorageChanged(Storage currentStorage);

        string QuickBarcodesPath { get; }

        List<StorageBarcode> LoadQuickBarcodes();

        void AddQuickBarcode(StorageBarcode barcode, int maxCapacity);

        void ClearQuickBarcodes();

        void RemoveQuickBarcode(StorageBarcode barcode);
    }
}