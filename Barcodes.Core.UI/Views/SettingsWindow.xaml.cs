namespace Barcodes.Core.UI.Views
{
    public partial class SettingsWindow : BaseWindow
    {
        public SettingsWindow(object dataContext)
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
