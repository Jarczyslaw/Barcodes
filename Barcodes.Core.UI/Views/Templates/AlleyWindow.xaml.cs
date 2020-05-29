namespace Barcodes.Core.UI.Views.Templates
{
    public partial class AlleyWindow : BaseWindow
    {
        public AlleyWindow(object dataContext)
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