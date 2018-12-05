using Barcodes.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Barcodes.Core.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : UserControl
    {
        public ShellView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            KeyDown += ShellView_KeyDown;      
        }

        private void ShellView_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(DataContext is ShellViewModel viewModel))
                return;

            if (e.Key == Key.F5)
                viewModel.Barcodes.GenerateRandomBarcodeCommand.Execute();
        }
    }
}
