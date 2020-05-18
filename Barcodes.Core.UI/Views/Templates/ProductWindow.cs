namespace Barcodes.Core.UI.Views.Templates
{
    public class ProductWindow : BaseTextWindow
    {
        public ProductWindow(object dataContext)
            : base(dataContext)
        {
            Title = "Barcodes - Product";
            Header = "Product data";
            Label = "Product code (EAN, 13 digits):";
            SetValueBinding("ProductCode");
        }
    }
}