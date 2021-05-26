using System;
using System.Globalization;
using System.Windows.Data;

namespace Barcodes.Core.UI.Converters
{
    public class DescriptionVisibleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visible = (bool)value;
            return "Description visible: " + (visible ? "Yes" : "No");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}