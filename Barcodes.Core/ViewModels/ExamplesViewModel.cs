using Barcodes.Services.Generator;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace Barcodes.Core.ViewModels
{
    public class ExamplesViewModel : BindableBase
    {
        private ObservableCollection<BarcodeViewModel> barcodes;

        private readonly IGeneratorService generatorService;

        public ExamplesViewModel(IGeneratorService generatorService)
        {
            this.generatorService = generatorService;
            CreateExamples();
        }

        public ObservableCollection<BarcodeViewModel> Barcodes
        {
            get => barcodes;
            set => SetProperty(ref barcodes, value);
        }

        private void CreateExamples()
        {

        }
    }
}
