using Barcodes.Core.Extensions;
using Barcodes.Services.DocExport;
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

        public BarcodeViewModel(BarcodeViewModel barcodeViewModel)
        {
            Title = barcodeViewModel.Title;
            Description = barcodeViewModel.Description;
            Barcode = barcodeViewModel.Barcode.Clone();
            GenerationData = new GenerationData(barcodeViewModel.GenerationData);
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

        public StorageBarcode ToStorage()
        {
            var result = GenerationData.ToStorageBarcode();
            result.Title = Title;
            result.Description = Description;
            return result;
        }

        public DocBarcodeData ToDocBarcodeData()
        {
            return new DocBarcodeData
            {
                Title = Title,
                Data = GenerationData.Data,
                Barcode = Barcode
            };
        }
    }
}