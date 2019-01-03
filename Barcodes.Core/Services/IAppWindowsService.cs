namespace Barcodes.Core.Services
{
    public interface IAppWindowsService
    {
        void OpenBarcodeWindow(object barcodeViewModel);
        string OpenNmvsProductWindow();
        string OpenEan128ProductWindow();
        void ShowHelpWindow();
    }
}
