using Barcodes.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        public HelpWindow()
        {
            InitializeComponent();
            ShowVersion();
        }

        private void HelpWindow_Loaded(object sender, RoutedEventArgs e)
        {
            KeyDown += HelpWindow_KeyDown;
        }

        private void ShowVersion()
        {
            var appVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
            tbVersion.Text = $"Version: {appVersion}";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Hyperlink_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            try
            {
                var hyperlink = (Hyperlink)sender;
                Process.Start(hyperlink.NavigateUri.ToString());
            }
            catch { }
        }

        private void HelpWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(DataContext is HelpViewModel viewModel))
                return;

            if (e.Key == Key.F5)
                viewModel.GenerateRandomBarcodeCommand.Execute();
        }
    }
}
