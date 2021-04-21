using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace JToolbox.WPF.UI.Converters
{
    public abstract class SingleConverterBase<T> : MarkupExtension, IValueConverter
       where T : class, new()
    {
        private static T Instance;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance ?? (Instance = new T());
        }

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }
}