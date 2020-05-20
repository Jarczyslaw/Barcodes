namespace Barcodes.Core.UI.Views
{
    public partial class RawSettingsWindow : BaseWindow
    {
        public RawSettingsWindow(object dataContext)
            : base(dataContext)
        {
            InitializeComponent();
        }

        protected override void OnViewShown()
        {
            base.OnViewShown();
            tbInputValue.Focus();
        }
    }
}
