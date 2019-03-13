namespace Barcodes.Core.UI.Views
{
    public partial class GenerationWindow : BaseWindow
    {
        public GenerationWindow(object dataContext)
            : base(dataContext)
        {
            InitializeComponent();
        }

        protected override void OnViewShown()
        {
            base.OnViewShown();
            generationView.tbTitle.Focus();
        }
    }
}
