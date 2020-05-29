namespace Barcodes.Core.UI.Views
{
    public partial class WorkspaceWindow : BaseWindow
    {
        public WorkspaceWindow()
        {
            InitializeComponent();
        }

        protected override void OnViewShown()
        {
            base.OnViewShown();
            BringToFront();
        }
    }
}