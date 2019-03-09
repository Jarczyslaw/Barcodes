using Barcodes.Core.ViewModels;
using Barcodes.Services.Generator;
using Barcodes.Services.Storage;

namespace Barcodes.Core.Extensions
{
    public static class StorageBarcodeExtensions
    {
        public static BarcodeViewModel ToBarcode(this StorageBarcode storageBarcode, IGeneratorService generator)
        {
            var barcodeData = new GenerationData
            {
                Data = storageBarcode.Data,
                Type = storageBarcode.Type,
                DefaultSize = storageBarcode.DefaultSize,
                ValidateCodeText = false,
                Width = storageBarcode.Width,
                Height = storageBarcode.Height
            };

            return new BarcodeViewModel(barcodeData)
            {
                Title = storageBarcode.Title,
                Barcode = generator.CreateBarcode(barcodeData)
            };
        }
    }
}
