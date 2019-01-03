using Barcodes.Core.Events;
using Prism.Events;
using System.Windows;

namespace Barcodes.Core.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ShellWindow : Window
    {
        public ShellWindow(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<CloseEvent>().Subscribe(Close, ThreadOption.UIThread, false);

            InitializeComponent();
        }
    }
}
