using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Services.Storage
{
    public class StorageWorkspace
    {
        public string Title { get; set; }
        public bool Default { get; set; }
        public List<StorageBarcode> Barcodes { get; set; }
    }
}
