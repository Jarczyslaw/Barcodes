namespace Barcodes.Core.UI.Views.Templates
{
    public partial class WarehouseAndStationWindow : BaseWindow
    {
        public WarehouseAndStationWindow(object dataContext)
            : base(dataContext)
        {
            InitializeComponent();
        }

        protected override void OnViewShown()
        {
            base.OnViewShown();
            iudWarehouse.Focus();
        }
    }
}
