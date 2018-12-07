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
        public bool DefaultSize { get; set; } = true;
        public int MinWidth { get; set; } = 250;
        public int Width { get; set; }
        public int Height { get; set; }
        public string Data { get; set; }
        public bool ShowData { get; set; } = false;
    }
}
