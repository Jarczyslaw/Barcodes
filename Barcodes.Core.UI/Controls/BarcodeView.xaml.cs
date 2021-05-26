using JToolbox.WPF.UI.Converters;
using System.Windows;
using System.Windows.Controls;

namespace Barcodes.Core.UI.Controls
{
    public partial class BarcodeView : UserControl
    {
        public static readonly DependencyProperty BarcodeVisibleProperty =
            DependencyProperty.Register(nameof(BarcodeVisible), typeof(bool), typeof(BarcodeView), new FrameworkPropertyMetadata(true, OnBarcodeVisiblePropertyChanged));

        public static readonly DependencyProperty DescriptionVisibleProperty =
           DependencyProperty.Register(nameof(DescriptionVisible), typeof(bool), typeof(BarcodeView), new FrameworkPropertyMetadata(true, OnDescriptionVisiblePropertyChanged));


        private static BoolToCollapsedConverter boolToCollapsedConverter = new BoolToCollapsedConverter();

        public BarcodeView()
        {
            InitializeComponent();
        }

        public bool BarcodeVisible
        {
            get => (bool)GetValue(BarcodeVisibleProperty);
            set => SetValue(BarcodeVisibleProperty, value);
        }

        public bool DescriptionVisible
        {
            get => (bool)GetValue(DescriptionVisibleProperty);
            set => SetValue(DescriptionVisibleProperty, value);
        }

        private static void OnBarcodeVisiblePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var barcodeView = source as BarcodeView;
            var barcodeVisible = (bool)e.NewValue;
            barcodeView.imgBarcode.Visibility = boolToCollapsedConverter.Convert(barcodeVisible);
        }

        private static void OnDescriptionVisiblePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var barcodeView = source as BarcodeView;
            var descriptionVisible = (bool)e.NewValue;
            barcodeView.lblDescription.Visibility =
                barcodeView.tbDescription.Visibility = boolToCollapsedConverter.Convert(descriptionVisible);
        }
    }
}