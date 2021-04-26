using System.Windows;

namespace JToolbox.WPF.UI.Converters
{
    public class BoolToHiddenConverter : BoolToValueConverter<Visibility>
    {
        public BoolToHiddenConverter()
            : base(Visibility.Visible, Visibility.Hidden)
        {
        }
    }
}