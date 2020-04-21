using Barcodes.SingleInstance;
using System.Windows;
using System.Windows.Threading;

namespace Barcodes.Core.UI.Views
{
    public partial class ShellWindow : BaseWindow
    {
        public ShellWindow(SingleInstanceManager singleInstanceManager)
        {
            InitializeComponent();
            KeyDownHandlerEnabled = true;
            singleInstanceManager.OnNewInstance += SingleInstanceManager_OnNewInstance;
        }

        private void SingleInstanceManager_OnNewInstance()
        {
            Application.Current?.Dispatcher.Invoke(BringToFront, DispatcherPriority.Background);
        }
    }
}