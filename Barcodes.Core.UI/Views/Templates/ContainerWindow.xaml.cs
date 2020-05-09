namespace Barcodes.Core.UI.Views.Templates
{
    public partial class ContainerWindow : BaseWindow
    {
        public ContainerWindow(object dataContext)
            : base(dataContext)
        {
            InitializeComponent();
        }

        protected override void OnViewShown()
        {
            base.OnViewShown();
            iudContainerNumber.Focus();
        }
    }
}
