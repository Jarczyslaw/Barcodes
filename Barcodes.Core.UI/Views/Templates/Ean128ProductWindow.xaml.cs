namespace Barcodes.Core.UI.Views.Templates
{
    public partial class Ean128ProductWindow : BaseWindow
    {
        public Ean128ProductWindow(object dataContext)
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
