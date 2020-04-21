using System.Windows;
using System.Windows.Input;

namespace Barcodes.Core.UI.Views
{
    public partial class AntiKeyProtectionWindow : BaseWindow
    {
        public AntiKeyProtectionWindow()
            : base(null)
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void ShowWarning(Window owner, Key key)
        {
            tbKeyWarning.Text = $"{key} key detected!";
            Owner = owner;
            ShowDialog();
        }
    }
}
