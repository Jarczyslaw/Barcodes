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

        private List<StorageBarcode> quickBarcodes = new List<StorageBarcode>();

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
            if (!File.Exists(QuickBarcodesPath))
            {
                return null;
            }

            return Serializer.FromFile<List<StorageBarcode>>(QuickBarcodesPath);
        }

        public void AddQuickBarcode(StorageBarcode barcode, int maxCapacity)
        {
            quickBarcodes.Add(barcode);
            if (quickBarcodes.Count > maxCapacity)
            {
                quickBarcodes.RemoveRange(0, quickBarcodes.Count - maxCapacity);
            }
            Serializer.ToFile(quickBarcodes, QuickBarcodesPath);
        }
    }
}
