using Barcodes.Core.Common;
using System.Windows;

namespace Barcodes.Core.UI.Views.AdditionalInput
{
    public partial class NmvsProductWindow : Window
    {
        public NmvsProductWindow(IClosable dataContext)
        {
            InitializeComponent();

            DataContext = dataContext;
            dataContext.CloseAction = () => Close();
        }
    }
}
