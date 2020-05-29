namespace Barcodes.Core.UI.Views
{
    public partial class BarcodeWindow : BaseWindow
    {
        public BarcodeWindow(object dataContext)
            : base(dataContext)
        {
            InitializeComponent();
        }

        protected override void OnViewShown()
        {
            base.OnViewShown();
            BringToFront();
        }
    }
}