using Barcodes.Core.ViewModels;

namespace Barcodes.Core.Services.StateSaver.States
{
    public class GenerationViewModelState
    {
        public string Title { get; set; }
        public int SelectedAdditionalInputIndex { get; set; }
        public string Data { get; set; }
        public bool ValidateCodeText { get; set; }
        public bool DefaultSize { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}