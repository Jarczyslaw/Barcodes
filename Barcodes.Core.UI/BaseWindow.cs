using Barcodes.Core.Common;
using System;
using System.ComponentModel;
using System.Windows;

namespace Barcodes.Core.UI
{
    public class BaseWindow : Window
    {
        private bool windowRendered;

        public BaseWindow()
        {
        }

        public BaseWindow(object dataContext)
        {
            DataContext = dataContext;
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            if (windowRendered)
            {
                return;
            }

            windowRendered = true;
            OnViewShown();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (DataContext is IOnClosingAware closingAware)
            {
                e.Cancel = closingAware.OnClosing();
            }
        }

        protected virtual void OnViewShown()
        {
            (DataContext as IOnShowAware)?.OnShow();
            if (DataContext is ICloseSource closeAware)
            {
                closeAware.OnClose += Close;
            }
        }
    }
}
