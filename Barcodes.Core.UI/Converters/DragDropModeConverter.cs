using Barcodes.Core.ViewModels;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Barcodes.Core.UI.Converters
{
    public class DragDropModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var mode = value as DragDropModeViewModel;
            if (mode != null)
            {
                return "Drag & drop mode: " + mode.Display;
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}