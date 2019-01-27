using Barcodes.Core.Events;
using Prism.Events;
using System.Windows;

namespace Barcodes.Core.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GenerationWindow : Window
    {
        public GenerationWindow(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<GenerationWindowClose>().Subscribe(Close, ThreadOption.UIThread, false);

            InitializeComponent();
        }
    }
}
