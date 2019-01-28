using Barcodes.Services.Generator;
using Prism.Mvvm;
using System;

namespace Barcodes.Core.ViewModels
{
    public class BarcodeTypeViewModel : BindableBase
    {
        public BarcodeTypeViewModel(BarcodeType type)
        {
            Type = type;
        }

        public string TypeTitle
        {
            get => Type.ToString();
        }

        public BarcodeType Type { get; set; }
    }
}
