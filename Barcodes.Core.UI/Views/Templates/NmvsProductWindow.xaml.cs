namespace Barcodes.Core.UI.Views.Templates
{
    public partial class NmvsProductWindow : BaseWindow
    {
        public NmvsProductWindow(object dataContext)
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
