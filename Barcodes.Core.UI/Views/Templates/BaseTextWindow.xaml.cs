using System.Windows.Controls;
using System.Windows.Data;

namespace Barcodes.Core.UI.Views.Templates
{
    public partial class BaseTextWindow : BaseWindow
    {
        public BaseTextWindow(object dataContext)
            : base(dataContext)
        {
            InitializeComponent();
        }

        public string Header
        {
            get => (string)gbHeader.Header;
            set => gbHeader.Header = value;
        }

        public string Label
        {
            get => (string)lblLabel.Content;
            set => lblLabel.Content = value;
        }

        protected void SetValueBinding(string propertyName)
        {
            var binding = new Binding(propertyName)
            {
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            tbValue.SetBinding(TextBox.TextProperty, binding);
        }

        protected override void OnViewShown()
        {
            base.OnViewShown();
            tbValue.Focus();
        }
    }
}
