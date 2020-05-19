namespace Barcodes.Core.UI.Views.Templates
{
    public class ContainerWindow : BaseNumericWindow
    {
        public ContainerWindow(object dataContext)
            : base(dataContext)
        {
            Title = "Barcodes - Container";
            Header = "Container data";
            Label = "Container number (up to 7 digits):";
            MinimumValue = 1;
            MaximumValue = 9999999;
            ShowUpDownButtons = false;
            SetValueBinding("ContainerNumber");
        }
    }
}