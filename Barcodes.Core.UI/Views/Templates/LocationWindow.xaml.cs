namespace Barcodes.Core.UI.Views.Templates
{
    public partial class LocationWindow : BaseWindow
    {
        public LocationWindow(object dataContext)
            : base(dataContext)
        {
            InitializeComponent();
        }

        protected override void OnViewShown()
        {
            base.OnViewShown();
            tbLocation.Focus();
        }
    }
}
