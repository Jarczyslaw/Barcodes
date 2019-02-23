using Barcodes.Core.Common.Events;
using Prism.Events;
using System.Windows;

namespace Barcodes.Core.UI.Views
{
    public partial class ShellWindow : Window
    {
        public ShellWindow(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<ShellWindowClose>().Subscribe(Close, ThreadOption.UIThread, false);

            InitializeComponent();
        }
    }
}
