using Aspose.BarCode.Generation;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Core.ViewModels
{
    public class BarcodeTypeViewModel : BindableBase
    {
        public string TypeTitle { get; set; }
        public SymbologyEncodeType Type { get; set; }
        public Func<string> ExtraInput { get; set; }
    }
}
