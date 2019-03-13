namespace Barcodes.Core.UI.Views
{
    public partial class InputWindow : BaseWindow
    {
        public InputWindow(object dataContext)
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
