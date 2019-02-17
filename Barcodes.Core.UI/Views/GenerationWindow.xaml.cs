using Barcodes.Core.Common.Events;
using Prism.Events;
using System.Windows;

namespace Barcodes.Core.UI.Views
{
    public partial class GenerationWindow : Window
    {
        public GenerationWindow(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<GenerationWindowClose>().Subscribe(Close, ThreadOption.UIThread, false);
            eventAggregator.GetEvent<GenerationWindowClose>().Subscribe(Close, ThreadOption.UIThread, false);

            InitializeComponent();
        }
    }
}
