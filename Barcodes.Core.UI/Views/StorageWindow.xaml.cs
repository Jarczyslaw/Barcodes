using Barcodes.Core.ViewModels;
using System.Windows.Controls;

namespace Barcodes.Core.UI.Views
{
    public partial class StorageWindow : BaseWindow
    {
        public StorageWindow(object dataContext)
            : base(dataContext)
        {
            InitializeComponent();
        }

        private void TabItem_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ((TabItem)sender).IsSelected = true;
        }
    }
}