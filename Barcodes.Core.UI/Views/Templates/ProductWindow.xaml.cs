namespace Barcodes.Core.UI.Views.Templates
{
    public partial class ProductWindow : BaseWindow
    {
        public ProductWindow(object dataContext)
            : base(dataContext)
        {
            InitializeComponent();
        }

        protected override void OnViewShown()
        {
            base.OnViewShown();
            tbProductCode.Focus();
        }
    }
}
