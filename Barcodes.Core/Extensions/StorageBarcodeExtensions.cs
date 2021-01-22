using Barcodes.Core.ViewModels;
using Barcodes.Services.Generator;
using Barcodes.Services.Storage;

namespace Barcodes.Core.Extensions
{
    public static class StorageBarcodeExtensions
    {
        public static BarcodeViewModel ToBarcode(this StorageBarcode storageBarcode, IGeneratorService generator)
        {
            var barcodeData = ToGenerationData(storageBarcode);
            return new BarcodeViewModel(barcodeData)
            {
                Title = storageBarcode.Title,
                Barcode = generator.CreateBarcode(barcodeData)
            };
        }

        public static GenerationData ToGenerationData(this StorageBarcode @this)
        {
            return new GenerationData
            {
                Data = @this.Data,
                Type = @this.Type,
                DefaultSize = @this.DefaultSize,
                ValidateCodeText = false,
                Width = @this.Width,
                Height = @this.Height
            };
        }
    }
}
