using Barcodes.Core.Common.Events;
using Prism.Events;
using System.Windows;

namespace Barcodes.Core.UI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ShellWindow : Window
    {
        public ShellWindow(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<ShellWindowClose>().Subscribe(Close, ThreadOption.UIThread, false);

            InitializeComponent();
        }
    }
}
