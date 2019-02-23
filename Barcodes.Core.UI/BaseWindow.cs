using Barcodes.Core.Common;
using System;
using System.Windows;

namespace Barcodes.Core.UI
{
    public class BaseWindow : Window
    {
        private bool windowRendered;

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

        private void OnViewShown()
        {
            (DataContext as IShowAware)?.OnShow();
            if (DataContext is ICloseAware closeAware)
            {
                closeAware.OnClose += Close;
            }
        }
    }
}
