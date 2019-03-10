using System.Windows;
using System.Windows.Controls;

namespace Barcodes.Core.UI.Controls
{
    public partial class BarcodeView : UserControl
    {
        public static readonly DependencyProperty BarcodeVisibleProperty =
            DependencyProperty.Register(nameof(BarcodeVisible), typeof(bool), typeof(BarcodeView), new FrameworkPropertyMetadata(true, OnBarcodeVisiblePropertyChanged));

        public BarcodeView()
        {
            InitializeComponent();
        }

        public bool BarcodeVisible
        {
            get => (bool)GetValue(BarcodeVisibleProperty);
            set => SetValue(BarcodeVisibleProperty, value);
        }

        private static void OnBarcodeVisiblePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var barcodeView = source as BarcodeView;
            var barcodeVisible = (bool)e.NewValue;
            barcodeView.barcodeImage.Visibility = barcodeVisible ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
