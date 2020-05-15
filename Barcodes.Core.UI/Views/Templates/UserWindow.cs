namespace Barcodes.Core.UI.Views.Templates
{
    public class UserWindow : BaseNumericWindow
    {
        public UserWindow(object dataContext)
            : base(dataContext)
        {
            Title = "Barcodes - User";
            Header = "User data";
            Label = "Number (5 digits):";
            SetValueBinding("Number");
        }
    }
}