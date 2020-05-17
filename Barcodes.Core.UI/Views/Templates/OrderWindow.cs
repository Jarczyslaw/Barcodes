namespace Barcodes.Core.UI.Views.Templates
{
    public class OrderWindow : BaseOrderWindow
    {
        public OrderWindow(object dataContext)
            : base(dataContext)
        {
            Title = "Barcodes - Order";
            Header = "Order data";
            OrderIdTitle = "Order ID (up to 9 digits):";
            OrderIdMaximum = 999999999;
        }
    }
}