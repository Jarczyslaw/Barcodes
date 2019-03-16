using Barcodes.Services.Generator;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace Barcodes.Core.ViewModels
{
    public class ExamplesViewModel : BindableBase
    {
        private ObservableCollection<BarcodeViewModel> barcodes;

        public ExamplesViewModel(IGeneratorService generatorService)
        {
            Barcodes = BarcodeExamples.CreateExamples(generatorService);
        }

        public ObservableCollection<BarcodeViewModel> Barcodes
        {
            get => barcodes;
            set => SetProperty(ref barcodes, value);
        }
    }
}
