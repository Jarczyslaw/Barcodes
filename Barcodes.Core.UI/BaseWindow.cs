using Barcodes.Core.Common;
using Barcodes.Core.UI.Views;
using Barcodes.Services.AppSettings;
using CommonServiceLocator;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Barcodes.Core.UI
{
    public class BaseWindow : Window
    {
        private bool windowRendered;

        private readonly IAppSettingsService appSettingsService;
        protected bool keyDownHandlerEnabled = true;

        public BaseWindow()
        {
            appSettingsService = ServiceLocator.Current.TryResolve<IAppSettingsService>();

            KeyDown += BaseWindow_KeyDown;
        }

        public BaseWindow(object dataContext)
            : this()
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

        private void BaseWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (keyDownHandlerEnabled && e.Key == Key.F5
                && appSettingsService?.AppSettings.F5Protection == true)
            {
                new F5ProtectionWindow().ShowWarning(this);
            }
        }

        public void BringToFront()
        {
            if (!IsVisible)
            {
                Show();
            }

            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
            }

            Activate();
            Topmost = true;
            Topmost = false;
            Focus();
        }
    }
}