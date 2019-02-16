using System.Windows;

namespace Barcodes.Core.Views.AdditionalInput
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Ean128ProductWindow : Window
    {
        public Ean128ProductWindow(IClosable dataContext)
        {
            InitializeComponent();

            DataContext = dataContext;
            dataContext.CloseAction = () => Close();
        }
    }
}
