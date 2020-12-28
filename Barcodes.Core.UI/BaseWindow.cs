using Barcodes.Core.Abstraction;
using Barcodes.Core.Common;
using Barcodes.Core.UI.Views;
using Barcodes.Services.AppSettings;
using Barcodes.Services.Logging;
using Barcodes.Utils;
using Prism.Ioc;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Barcodes.Core.UI
{
    public class BaseWindow : Window
    {
        private bool windowRendered;
        private bool windowInitialized;

        private readonly IAppSettingsService appSettingsService;
        private readonly IAppDialogsService appDialogsService;
        private readonly ILoggerService loggerService;

        public BaseWindow()
        {
            appSettingsService = ContainerLocator.Container.Resolve<IAppSettingsService>();
            appDialogsService = ContainerLocator.Container.Resolve<IAppDialogsService>();
            loggerService = ContainerLocator.Container.Resolve<ILoggerService>();

            KeyDown += BaseWindow_KeyDown;
        }

        public BaseWindow(object dataContext)
            : this()
        {
            DataContext = dataContext;
        }

        public bool KeyDownHandlerEnabled { get; set; }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            if (windowInitialized)
            {
                return;
            }

            windowInitialized = true;
            OnViewInitialized();
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

        protected virtual void OnViewInitialized()
        {
            if (DataContext is ICloseSource closeAware)
            {
                closeAware.OnClose += Close;
            }
        }

        protected virtual void OnViewShown()
        {
            (DataContext as IOnShowAware)?.OnShow();
        }

        private void BaseWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (KeyDownHandlerEnabled)
            {
                var handled = false;
                if (DataContext is IOnKeyDownAware keyDownAware)
                {
                    handled = keyDownAware.OnKeyDown(e);
                }

                if (!handled)
                {
                    try
                    {
                        if (KeyParser.Check(appSettingsService.AppSettings.AntiKeyProtection, e.Key))
                        {
                            new AntiKeyProtectionWindow().ShowWarning(this, e.Key);
                        }
                    }
                    catch (Exception exc)
                    {
                        var message = "Error checking keys";
                        appDialogsService.ShowException(message, exc);
                        loggerService.Error(exc, message);
                    }
                }
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