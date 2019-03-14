using Barcodes.Services.Generator;
using Barcodes.Services.Storage;
using Prism.Mvvm;
using System.Windows.Media.Imaging;

namespace Barcodes.Core.ViewModels
{
    public class BarcodeViewModel : BindableBase
    {
        private string title;
        private string description;
        private BitmapSource barcode;

        public BarcodeViewModel(GenerationData generationData)
        {
            GenerationData = generationData;
        }

        public GenerationData GenerationData { get; }

        public string HeaderTitle
        {
            get { return $"Barcodes - {Title}"; }
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string TypeTitle
        {
            get => GenerationData.Type.ToString();
        }

        public BitmapSource Barcode
        {
            get => barcode;
            set => SetProperty(ref barcode, value);
        }

        public StorageBarcode ToStorage()
        {
            return new StorageBarcode
            {
                Data = GenerationData.Data,
                Title = Title,
                Type = GenerationData.Type,
                Width = GenerationData.Width,
                Height = GenerationData.Height,
                DefaultSize = GenerationData.DefaultSize
            };
        }
    }
}
