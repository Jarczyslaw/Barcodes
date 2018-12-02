using Aspose.BarCode.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Services.Generator
{
    public class BarcodeData
    {
        public BarcodeType Type { get; set; }
        public int? MinWidth { get; set; } = 300;
        public string Data { get; set; }
        public bool ShowData { get; set; } = false;
    }
}
