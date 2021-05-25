using Barcodes.Core.Common;
using Barcodes.Utils;
using System;
using System.Collections.Generic;
using System.IO;

namespace Barcodes.Services.Storage
{
    public class StorageService : IStorageService
    {
        private Storage currentStorage = new Storage();

        public string QuickBarcodesPath { get; } = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"quickBarcodes.{FileExtensions.QuickBarcodes}");

        private List<StorageBarcode> quickBarcodes;

        public Storage Load(string filePath, bool setCurrentStorage)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            var loadedStorage = Serializer.FromFile<Storage>(filePath);
            if (setCurrentStorage)
            {
                currentStorage = loadedStorage;
            }
            return loadedStorage;
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

        public List<StorageBarcode> ImportBarcodes(string filePath)
        {
            return Serializer.FromFile<List<StorageBarcode>>(filePath);
        }

        public void ExportBarcodes(string filePath, List<StorageBarcode> barcode)
        {
            Serializer.ToFile(barcode, filePath);
        }

        public bool StorageChanged(Storage currentStorage)
        {
            return !ObjectUtils.ObjectEquality(this.currentStorage, currentStorage);
        }

        public List<StorageBarcode> LoadQuickBarcodes()
        {
            if (quickBarcodes == null && File.Exists(QuickBarcodesPath))
            {
                quickBarcodes = Serializer.FromFile<List<StorageBarcode>>(QuickBarcodesPath);
            }
            return quickBarcodes;
        }

        public void AddQuickBarcode(StorageBarcode barcode, int maxCapacity)
        {
            if (quickBarcodes == null)
            {
                quickBarcodes = new List<StorageBarcode>();
            }

            quickBarcodes.RemoveAll(b => barcode.Data == b.Data && barcode.Type == b.Type);
            quickBarcodes.Insert(0, barcode);
            if (quickBarcodes.Count > maxCapacity)
            {
                quickBarcodes.RemoveRange(maxCapacity, quickBarcodes.Count - maxCapacity);
            }
            Serializer.ToFile(quickBarcodes, QuickBarcodesPath);
        }

        public void ClearQuickBarcodes()
        {
            quickBarcodes = null;
            if (File.Exists(QuickBarcodesPath))
            {
                File.Delete(QuickBarcodesPath);
            }
        }

        public void RemoveQuickBarcode(StorageBarcode barcode)
        {
            if (quickBarcodes.Remove(barcode))
            {
                Serializer.ToFile(quickBarcodes, QuickBarcodesPath);
            }
        }
    }
}