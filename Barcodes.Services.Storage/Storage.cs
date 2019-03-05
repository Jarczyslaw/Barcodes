using System;
using System.Collections.Generic;
using System.Linq;

namespace Barcodes.Services.Storage
{
    public class Storage : IEquatable<Storage>
    {
        public List<StorageWorkspace> Content { get; set; } = new List<StorageWorkspace>();

        public int BarcodesCount
        {
            get => Content.Sum(c => c.Barcodes.Count);
        }

        public int WorkspacesCount
        {
            get => Content.Count;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Storage otherStorage))
            {
                return false;
            }

            return Equals(otherStorage);
        }

        public bool Equals(Storage other)
        {
            return Content.SequenceEqual(other.Content);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                foreach (var workspace in Content)
                {
                    hash = (hash * 17) + workspace.GetHashCode();
                }
                return hash;
            }
        }
    }
}
