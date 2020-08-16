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
        private bool isChecked;
        private BitmapSource barcode;

        public BarcodeViewModel(GenerationData generationData)
        {
            GenerationData = generationData;
        }

        public BarcodeViewModel(BarcodeViewModel barcodeViewModel)
        {
            Title = barcodeViewModel.Title;
            Description = barcodeViewModel.Description;
            Barcode = barcodeViewModel.Barcode.Clone();
            GenerationData = barcodeViewModel.GenerationData;
        }

        public GenerationData GenerationData { get; }

        public string HeaderTitle
        {
            get { return $"Barcode - {Title}"; }
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

        public bool IsChecked
        {
            get => isChecked;
            set => SetProperty(ref isChecked, value);
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