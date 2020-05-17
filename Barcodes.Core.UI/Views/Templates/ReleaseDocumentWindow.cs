namespace Barcodes.Core.UI.Views.Templates
{
    public class ReleaseDocumentWindow : BaseOrderWindow
    {
        public ReleaseDocumentWindow(object dataContext)
            : base(dataContext)
        {
            Title = "Barcodes - Release document";
            Header = "Release document data";
            OrderIdTitle = "Order ID (7 digits):";
            OrderIdMaximum = 9999999;
        }
    }
}