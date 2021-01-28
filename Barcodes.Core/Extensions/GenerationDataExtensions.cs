using Barcodes.Extensions;
using Barcodes.Services.AppSettings;
using Barcodes.Services.Generator;
using Barcodes.Services.Storage;

namespace Barcodes.Core.Extensions
{
    public static class GenerationDataExtensions
    {
        public static StorageBarcode ToStorageBarcode(this GenerationData @this)
        {
            return new StorageBarcode
            {
                Width = @this.Width,
                Data = @this.Data,
                DefaultSize = @this.DefaultSize,
                Height = @this.Height,
                Type = @this.Type,
                Title = null
            };
        }

        public static GenerationSettings ToSettings(this GenerationData @this)
        {
            return new GenerationSettings
            {
                Width = @this.Width,
                DefaultSize = @this.DefaultSize,
                Height = @this.Height,
                Type = @this.Type,
                ValidateCodeText = @this.ValidateCodeText,
            };
        }

        public static string GetTitle(this GenerationData @this)
        {
            return $"{@this.Data.Sanitize(25)} - {@this.Type}";
        }
    }
}