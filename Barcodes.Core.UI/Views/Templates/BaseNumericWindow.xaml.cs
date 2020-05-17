using System.Windows.Data;
using Xceed.Wpf.Toolkit;

namespace Barcodes.Core.UI.Views.Templates
{
    public partial class BaseNumericWindow : BaseWindow
    {
        public BaseNumericWindow(object dataContext)
            : base(dataContext)
        {
            InitializeComponent();
        }

        public bool ShowUpDownButtons
        {
            get => iudValue.ShowButtonSpinner;
            set => iudValue.ShowButtonSpinner = value;
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

        public int MinimumValue
        {
            set => iudValue.Minimum = value;
        }

        public int MaximumValue
        {
            set => iudValue.Maximum = value;
        }

        protected void SetValueBinding(string propertyName)
        {
            var binding = new Binding(propertyName)
            {
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            };
            iudValue.SetBinding(IntegerUpDown.ValueProperty, binding);
        }

        protected override void OnViewShown()
        {
            base.OnViewShown();
            iudValue.Focus();
        }
    }
}