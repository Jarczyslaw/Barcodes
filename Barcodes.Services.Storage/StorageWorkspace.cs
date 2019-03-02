using System;
using System.Collections.Generic;
using System.Linq;

namespace Barcodes.Services.Storage
{
    public class StorageWorkspace : IEquatable<StorageWorkspace>
    {
        public string Title { get; set; }
        public bool Default { get; set; }
        public List<StorageBarcode> Barcodes { get; set; } = new List<StorageBarcode>();

        public override bool Equals(object obj)
        {
            if (!(obj is StorageWorkspace otherStorageWorkspace))
            {
                return false;
            }

            return Equals(otherStorageWorkspace);
        }

        public bool Equals(StorageWorkspace other)
        {
            return Title == other.Title
                && Default == other.Default
                && Barcodes.SequenceEqual(other.Barcodes);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = Title.GetHashCode();
                hash += 17 * Default.GetHashCode();
                foreach (var barcode in Barcodes)
                {
                    hash = (hash * 17) + barcode.GetHashCode();
                }
                return hash;
            }
        }
    }
}
