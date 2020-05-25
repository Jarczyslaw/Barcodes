namespace Barcodes.Core.UI.Views.Templates
{
    public partial class PaletteWindow : BaseWindow
    {
        public PaletteWindow(object dataContext)
            : base(dataContext)
        {
            InitializeComponent();
        }

        protected override void OnViewShown()
        {
            base.OnViewShown();
            iudValue.Focus();
        }
    }
}