namespace Barcodes.Core.UI.Views.Templates
{
    public partial class BaseOrderWindow : BaseWindow
    {
        public BaseOrderWindow(object dataContext)
            : base(dataContext)
        {
            InitializeComponent();
        }

        public string OrderIdTitle
        {
            get => (string)lblOrderId.Content;
            set => lblOrderId.Content = value;
        }

        public string Header
        {
            get => (string)gbHeader.Header;
            set => gbHeader.Header = value;
        }

        public int OrderIdMinimum
        {
            set => iudOrderId.Minimum = value;
        }

        public int OrderIdMaximum
        {
            set => iudOrderId.Maximum = value;
        }

        protected override void OnViewShown()
        {
            base.OnViewShown();
            iudOrderId.Focus();
        }
    }
}
