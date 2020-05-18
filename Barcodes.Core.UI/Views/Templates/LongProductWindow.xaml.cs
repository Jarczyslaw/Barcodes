namespace Barcodes.Core.UI.Views.Templates
{
    public partial class LongProductWindow : BaseWindow
    {
        public LongProductWindow(object dataContext)
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
