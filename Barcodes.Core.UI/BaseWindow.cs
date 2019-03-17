using Barcodes.Core.Common;
using Barcodes.Services.AppSettings;
using Barcodes.Services.Dialogs;
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
        private readonly IDialogsService dialogsService;

        public BaseWindow()
        {
            appSettingsService = ServiceLocator.Current.TryResolve<IAppSettingsService>();
            dialogsService = ServiceLocator.Current.TryResolve<IDialogsService>();

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
            if (e.Key == Key.F5 && appSettingsService?.AppSettings.F5Protection == true && dialogsService != null)
            {
                dialogsService.ShowWarning("Unwanted F5 detected!");
            }
        }
    }
}
