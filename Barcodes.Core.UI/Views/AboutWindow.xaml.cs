using Barcodes.Core.ViewModels;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace Barcodes.Core.UI.Views
{
    public partial class AboutWindow : BaseWindow
    {
        private readonly AboutViewModel viewModel;

        public AboutWindow(AboutViewModel aboutViewModel)
        {
            viewModel = aboutViewModel;
            DataContext = aboutViewModel;
            InitializeComponent();
        }

        private void AboutWindow_Loaded(object sender, RoutedEventArgs e)
        {
            KeyDown += AboutWindow_KeyDown;
        }

        private async void AboutWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                await viewModel.GenerateRandomBarcodeAsync();
            }
        }

        private async void ImgBarcode_MouseDown(object sender, MouseButtonEventArgs e)
        {
            await viewModel.GenerateRandomBarcodeAsync();
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            try
            {
                var hyperlink = (Hyperlink)sender;
                Process.Start(hyperlink.NavigateUri.ToString());
            }
            catch { }
        }
    }
}