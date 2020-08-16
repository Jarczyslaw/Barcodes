using Barcodes.Codes;

namespace Barcodes.Services.Generator
{
    public class GenerationData
    {
        public GenerationData()
        {
        }

        public GenerationData(GenerationData other)
        {
            Type = other.Type;
            DefaultSize = other.DefaultSize;
            MinWidth = other.MinWidth;
            Width = other.Width;
            Height = other.Height;
            Data = other.Data;
            ValidateCodeText = other.ValidateCodeText;
        }

        public BarcodeType Type { get; set; }
        public bool DefaultSize { get; set; } = true;
        public int MinWidth { get; } = 100;
        public int Width { get; set; }
        public int Height { get; set; }
        public string Data { get; set; }
        public bool ValidateCodeText { get; set; }
    }
}