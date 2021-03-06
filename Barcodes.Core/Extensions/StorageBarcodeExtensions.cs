﻿using Barcodes.Core.ViewModels;
using Barcodes.Extensions;
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
                Description = storageBarcode.Description,
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

        public static string GetTitle(this StorageBarcode @this)
        {
            return $"{@this.Data.Sanitize(25)} - {@this.Type}";
        }
    }
}
