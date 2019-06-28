using System.Windows;

namespace Barcodes.Core.UI.Views
{
    public partial class F5ProtectionWindow : BaseWindow
    {
        public F5ProtectionWindow()
            : base(null)
        {
            InitializeComponent();
            keyDownHandlerEnabled = false;
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Close();
        }

        public void ShowWarning(Window owner)
        {
            Owner = owner;
            ShowDialog();
        }
    }
}
