using Aspose.BarCode.Generation;
using Barcodes.Services.Generator;
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
        public string TypeTitle
        {
            get => Type.ToString();
        }
        public BarcodeType Type { get; set; }
        public Func<string> ExtraInput { get; set; }
    }
}
