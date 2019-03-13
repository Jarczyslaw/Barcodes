using System.Windows.Controls;

namespace Barcodes.Core.UI.Controls
{
    public class CustomTextBox : TextBox
    {
        public CustomTextBox()
        {
            GotFocus += CustomTextBox_GotFocus;
        }

        private void CustomTextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Text))
            {
                return;
            }

            CaretIndex = Text.Length;
        }
    }
}
