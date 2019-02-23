using Barcodes.Core.Common;
using System.Windows;

namespace Barcodes.Core.UI.Views.AdditionalInput
{
    public partial class NmvsProductWindow : Window
    {
        public NmvsProductWindow(ICloseAware dataContext)
        {
            InitializeComponent();

            DataContext = dataContext;
            dataContext.OnClose = () => Close();
        }
    }
}
