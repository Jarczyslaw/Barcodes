using JToolbox.WPF.Core.Awareness;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace JToolbox.WPF.UI
{
    public class WindowEvents
    {
        public WindowEvents(WindowBase window)
        {
            Window = window;
        }

        public WindowBase Window { get; }
        public bool WindowRendered { get; private set; }
        public bool WindowInitialized { get; private set; }
        private object DataContext => Window.DataContext;

        public void Attach()
        {
            Window.Initialized += Window_Initialized;
            Window.ContentRendered += Window_ContentRendered;
            Window.Closing += Window_Closing;
            Window.Closed += Window_Closed;
            Window.Loaded += Window_Loaded;
            Window.KeyDown += Window_KeyDown;
            Window.DataContextChanged += Window_DataContextChanged;
        }

        public void Detach()
        {
            Window.Initialized -= Window_Initialized;
            Window.ContentRendered -= Window_ContentRendered;
            Window.Closing -= Window_Closing;
            Window.Closed -= Window_Closed;
            Window.Loaded -= Window_Loaded;
            Window.KeyDown -= Window_KeyDown;
            Window.DataContextChanged -= Window_DataContextChanged;
        }

        private void SetupCloseEvent(object dataContext, bool attach)
        {
            if (dataContext is ICloseSource closeSource)
            {
                closeSource.OnClose -= Window.Close;
                if (attach)
                {
                    closeSource.OnClose += Window.Close;
                }
            }
        }

        private void SetupDataEvent(object dataContext, bool attach)
        {
            if (dataContext is IDataSource dataSource)
            {
                dataSource.OnData -= Window.OnData;
                if (attach)
                {
                    dataSource.OnData += Window.OnData;
                }
            }
        }

        private void SetupSourceEvents(object dataContext, bool attach)
        {
            SetupCloseEvent(dataContext, attach);
            SetupDataEvent(dataContext, attach);
        }

        #region Window events

        private void Window_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SetupSourceEvents(e.OldValue, false);
            SetupSourceEvents(e.NewValue, true);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            (DataContext as IOnClosedAware)?.OnClosed();
            SetupSourceEvents(DataContext, false);
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            if (!WindowInitialized)
            {
                (DataContext as IOnInitializedAware)?.OnInitialized();
                WindowInitialized = true;
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            if (!WindowRendered)
            {
                (DataContext as IOnShowAware)?.OnShow();
                WindowRendered = true;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (DataContext is IOnClosingAware closingAware)
            {
                e.Cancel = closingAware.OnClosing();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is IOnLoadedAware loadedAware)
            {
                loadedAware.OnLoaded();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (DataContext is IOnKeyDownAware keyDownAware)
            {
                keyDownAware.OnKeyDown(e);
            }
        }

        #endregion Window events
    }
}