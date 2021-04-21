using System.Windows;

namespace JToolbox.WPF.UI.Converters
{
    public class BoolToCollapsedConverter : BoolToValueConverter<Visibility>
    {
        public BoolToCollapsedConverter()
            : base(Visibility.Visible, Visibility.Collapsed)
        {
        }
    }
}