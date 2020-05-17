namespace Barcodes.Core.UI.Views.Templates
{
    public class UserWindow : BaseNumericWindow
    {
        public UserWindow(object dataContext)
            : base(dataContext)
        {
            Title = "Barcodes - User";
            Header = "User data";
            Label = "Number (up to 5 digits):";
            MinimumValue = 1;
            MaximumValue = 99999;
            SetValueBinding("Number");
        }
    }
}