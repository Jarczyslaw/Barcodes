using Barcodes.Services.AppSettings;

namespace Barcodes.Core.ViewModels
{
    public class DragDropModeViewModel
    {
        public DragDropModeViewModel(DragDropMode dragDropMode)
        {
            DragDropMode = dragDropMode;
        }

        public string Display
        {
            get
            {
                if (DragDropMode == DragDropMode.Arrangement)
                {
                    return "Arrangement";
                }
                else if (DragDropMode == DragDropMode.ImportExport)
                {
                    return "Import/export";
                }
                return string.Empty;
            }
        }

        public DragDropMode DragDropMode { get; }
    }
}