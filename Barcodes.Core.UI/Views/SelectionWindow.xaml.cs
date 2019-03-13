namespace Barcodes.Core.UI.Views
{
    public partial class SelectionWindow : BaseWindow
    {
        public SelectionWindow(object dataContext)
            : base(dataContext)
        {
            InitializeComponent();
        }

        protected override void OnViewShown()
        {
            base.OnViewShown();
            cbSelectedItem.Focus();
        }
    }
}
