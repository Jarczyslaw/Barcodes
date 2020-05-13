namespace Barcodes.Core.UI.Views.Templates
{
    public class OrderWindow : BaseOrderWindow
    {
        public OrderWindow(object dataContext)
            : base(dataContext)
        {
            Title = "Barcodes - Order";
            Header = "Order data";
            OrderIdTitle = "Order ID (9 digits):";
        }
    }
}