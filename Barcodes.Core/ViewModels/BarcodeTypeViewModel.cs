using Barcodes.Services.Generator;
using Prism.Mvvm;
using System;

namespace Barcodes.Core.ViewModels
{
    public class BarcodeTypeViewModel : BindableBase
    {
        public string TypeTitle
        {
            get => Type.ToString();
        }

        public BarcodeType Type { get; set; }

        public Func<string> AdditionalInput { get; set; }
    }
}
