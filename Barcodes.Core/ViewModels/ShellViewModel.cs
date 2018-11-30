using Barcodes.Services.Generator;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Barcodes.Core.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private BitmapSource randomBarcode;
        public BitmapSource RandomBarcode
        {
            get => randomBarcode;
            set => SetProperty(ref randomBarcode, value);
        }

        private readonly IBarcodesGeneratorService barcodesGenerator;

        public ShellViewModel(IBarcodesGeneratorService barcodesGenerator)
        {
            this.barcodesGenerator = barcodesGenerator;

            RandomBarcode = barcodesGenerator.CreateRandomBarcode(400, 400);
        }
    }
}
