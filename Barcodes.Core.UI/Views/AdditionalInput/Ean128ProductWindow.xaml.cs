using Barcodes.Core.Common;
using System.Windows;

namespace Barcodes.Core.UI.Views.AdditionalInput
{
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
