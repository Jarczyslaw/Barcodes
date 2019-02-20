using Barcodes.Core.ViewModels;
using Barcodes.Services.Generator;

namespace Barcodes.Core.Services
{
    public interface IAppWindowsService
    {
        void OpenBarcodeWindow(object barcodeViewModel);
        string OpenNmvsProductWindow(string nmvsData);
        string OpenEan128ProductWindow(string ean128Data);
        void ShowHelpWindow();
        GenerationViewModelResult ShowGenerationWindow(BarcodeResultViewModel barcode = null);
    }
}
