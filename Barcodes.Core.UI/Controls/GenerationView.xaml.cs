using JToolbox.WPF.UI.Converters;
using System.Windows;
using System.Windows.Controls;

namespace Barcodes.Core.UI.Controls
{
    public partial class GenerationView : UserControl
    {
        public static readonly DependencyProperty TitleVisibleProperty =
            DependencyProperty.Register(nameof(TitleVisible), typeof(bool), typeof(GenerationView), new FrameworkPropertyMetadata(true, OnTitleVisiblePropertyChanged));

        private static BoolToCollapsedConverter boolToCollapsedConverter = new BoolToCollapsedConverter();

        public GenerationView()
        {
            InitializeComponent();
        }

        public bool TitleVisible
        {
            get => (bool)GetValue(TitleVisibleProperty);
            set => SetValue(TitleVisibleProperty, value);
        }

        private static void OnTitleVisiblePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var barcodeView = source as GenerationView;
            var titleVisible = (bool)e.NewValue;
            barcodeView.lbTitle.Visibility =
                barcodeView.tbTitle.Visibility = boolToCollapsedConverter.Convert(titleVisible);
        }
    }
}