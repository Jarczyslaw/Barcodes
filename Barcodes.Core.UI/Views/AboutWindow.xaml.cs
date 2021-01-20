using Barcodes.Services.Generator;
using Barcodes.Utils;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace Barcodes.Core.UI.Views
{
    public partial class AboutWindow : BaseWindow
    {
        private readonly IGeneratorService barcodesGenerator;

        public AboutWindow(IGeneratorService barcodesGenerator)
        {
            this.barcodesGenerator = barcodesGenerator;

            InitializeComponent();
            ShowVersion();
        }

        private void AboutWindow_Loaded(object sender, RoutedEventArgs e)
        {
            KeyDown += AboutWindow_KeyDown;
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

        private async void AboutWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F5)
            {
                await GenerateRandomBarcode();
            }
        }

        public async Task GenerateRandomBarcode()
        {
            var randomText = RandomTexts.Get();
            imgBarcode.Source = await Task.Run(() => barcodesGenerator.CreateQRBarcode(300, randomText));
        }

        private async void ImgBarcode_MouseDown(object sender, MouseButtonEventArgs e)
        {
            await GenerateRandomBarcode();
        }
    }
}