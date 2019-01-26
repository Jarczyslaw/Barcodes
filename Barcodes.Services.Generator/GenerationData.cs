namespace Barcodes.Services.Generator
{
    public class GenerationData
    {
        public BarcodeType Type { get; set; }
        public bool DefaultSize { get; set; } = true;
        public int MinWidth { get; } = 200;
        public int Width { get; set; }
        public int Height { get; set; }
        public string Data { get; set; }
        public bool ShowData { get; set; }
        public bool ValidateCodeText { get; set; }
    }
}
