using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Barcodes.Core.UI.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public Visibility InvisibleValue { get; set; } = Visibility.Collapsed;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool val)
            {
                return Convert(val, InvisibleValue);
            }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static Visibility Convert(bool value, Visibility? invisibleValue)
        {
            invisibleValue = invisibleValue ?? Visibility.Collapsed;
            return value ? Visibility.Visible : invisibleValue.Value;
        }
    }
}
