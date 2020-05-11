namespace Barcodes.Core.UI.Views.Templates
{
    public class LocationWindow : BaseTextWindow
    {
        public LocationWindow(object dataContext)
            : base(dataContext)
        {
            Title = "Barcodes - Location";
            Header = "Location data";
            Label = "Location address (10 signs):";
            SetValueBinding("Location");
        }
    }
}