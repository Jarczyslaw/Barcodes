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
        public SymbologyEncodeType Type { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Data { get; set; }
        public bool ShowData { get; set; }
    }
}
