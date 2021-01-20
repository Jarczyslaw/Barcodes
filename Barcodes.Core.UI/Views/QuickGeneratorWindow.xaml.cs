namespace Barcodes.Core.UI.Views
{
    public partial class QuickGeneratorWindow : BaseWindow
    {
        public QuickGeneratorWindow(object dataContext)
            : base(dataContext)
        {
            InitializeComponent();
        }

        protected override void OnViewShown()
        {
            base.OnViewShown();
            generationView.tbData.Focus();
        }
    }
}
