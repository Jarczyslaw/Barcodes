namespace Barcodes.Core.UI.Views.Templates
{
    public class ContainerWindow : BaseNumericWindow
    {
        public ContainerWindow(object dataContext)
            : base(dataContext)
        {
            Title = "Barcodes - Container";
            Header = "Container data";
            Label = "Container number (7 digits):";
            SetValueBinding("ContainerNumber");
        }
    }
}