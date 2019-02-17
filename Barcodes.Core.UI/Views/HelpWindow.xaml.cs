using Barcodes.Services.Generator;
using Barcodes.Utils;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace Barcodes.Core.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {
        private readonly IGeneratorService barcodesGenerator;

        public HelpWindow(IGeneratorService barcodesGenerator)
        {
            this.barcodesGenerator = barcodesGenerator;

            InitializeComponent();
            ShowVersion();
        }

        private void HelpWindow_Loaded(object sender, RoutedEventArgs e)
        {
            KeyDown += HelpWindow_KeyDown;
            GenerateRandomBarcode();
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
            if (e.Key == Key.F5)
            {
                GenerateRandomBarcode();
            }
        }

        private void GenerateRandomBarcode()
        {
            var randomText = RandomTexts.Get();
            imgBarcode.Source = barcodesGenerator.CreateQRBarcode(300, randomText);
        }
    }
}
