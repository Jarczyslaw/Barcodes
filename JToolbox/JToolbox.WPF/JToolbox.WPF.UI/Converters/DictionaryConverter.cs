using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace JToolbox.WPF.UI.Converters
{
    public class DictionaryConverter<TKey, TValue> : IValueConverter
    {
        public Dictionary<TKey, TValue> Values { get; set; } = new Dictionary<TKey, TValue>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var key = (TKey)value;

            if (targetType != typeof(TValue) || key == null || !Values.ContainsKey(key))
                return default(TValue);

            return Values[key];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (TValue)value;

            if (targetType != typeof(TKey) || val == null || !Values.ContainsValue(val))
                return default(TKey);

            return Values.First(pair => pair.Value.Equals(val)).Key;
        }
    }
}