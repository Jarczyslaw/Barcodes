using Barcodes.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Services.Storage
{
    public class BarcodeStorageService : IBarcodeStorageService
    {
        public List<BarcodeStorageEntry> Load(string filePath, bool throwException = false)
        {
            try
            {
                return Serializer.FromFile<List<BarcodeStorageEntry>>(filePath);
            }
            catch
            {
                if (throwException)
                    throw;
                return null;
            }
        }

        public void Save(string filePath, List<BarcodeStorageEntry> barcodes)
        {
            Serializer.ToFile(barcodes, filePath);
        }
    }
}
