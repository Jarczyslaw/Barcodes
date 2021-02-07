using Barcodes.Core.ViewModels;
using System.Linq;
using System.Windows.Controls;

namespace Barcodes.Core.UI.Views
{
    public partial class StorageWindow : BaseWindow
    {
        public StorageWindow(StorageViewModel storageViewModel)
        {
            DataContext = storageViewModel;
            InitializeComponent();
        }

        private void TabItem_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ((TabItem)sender).IsSelected = true;
        }

        private void lvBarcodes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is StorageViewModel storageViewModel)
            {
                var listView = sender as ListView;
                storageViewModel.SelectedBarcodes = listView.SelectedItems
                    .Cast<BarcodeViewModel>()
                    .ToList();
            }
        }
    }
}