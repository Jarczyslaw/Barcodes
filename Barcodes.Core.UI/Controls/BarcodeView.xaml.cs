﻿using System.Windows;
using System.Windows.Controls;
using Barcodes.Core.UI.Converters;

namespace Barcodes.Core.UI.Controls
{
    public partial class BarcodeView : UserControl
    {
        public static readonly DependencyProperty BarcodeVisibleProperty =
            DependencyProperty.Register(nameof(BarcodeVisible), typeof(bool), typeof(BarcodeView), new FrameworkPropertyMetadata(true, OnBarcodeVisiblePropertyChanged));

        public static readonly DependencyProperty IsCheckedEnabledProperty =
            DependencyProperty.Register(nameof(IsCheckedEnabled), typeof(bool), typeof(BarcodeView), new FrameworkPropertyMetadata(true, OnIsCheckedEnabledPropertyChanged));

        public BarcodeView()
        {
            InitializeComponent();
        }

        public bool BarcodeVisible
        {
            get => (bool)GetValue(BarcodeVisibleProperty);
            set => SetValue(BarcodeVisibleProperty, value);
        }

        public bool IsCheckedEnabled
        {
            get => (bool)GetValue(IsCheckedEnabledProperty);
            set => SetValue(IsCheckedEnabledProperty, value);
        }

        public bool DescriptionVisible
        {
            set
            {
                var visibility = BoolToVisibilityConverter.Convert(value, Visibility.Collapsed);
                lblDescription.Visibility = visibility;
                tbDescription.Visibility = visibility;
            }
        }

        private static void OnBarcodeVisiblePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var barcodeView = source as BarcodeView;
            var barcodeVisible = (bool)e.NewValue;
            barcodeView.imgBarcode.Visibility = BoolToVisibilityConverter.Convert(barcodeVisible, Visibility.Collapsed);
        }

        private static void OnIsCheckedEnabledPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var barcodeView = source as BarcodeView;
            var isCheckedEnabled = (bool)e.NewValue;
            barcodeView.chbIsChecked.Visibility = BoolToVisibilityConverter.Convert(isCheckedEnabled, Visibility.Collapsed);
        }
    }
}
