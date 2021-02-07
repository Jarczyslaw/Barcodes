﻿using Barcodes.Core.ViewModels;
using Barcodes.SingleInstance;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Barcodes.Core.UI.Views
{
    public partial class ShellWindow : BaseWindow
    {
        public ShellWindow(SingleInstanceManager singleInstanceManager, ShellViewModel shellViewModel)
        {
            DataContext = shellViewModel;
            InitializeComponent();
            KeyDownHandlerEnabled = true;
            singleInstanceManager.OnNewInstance += SingleInstanceManager_OnNewInstance;
        }

        private void SingleInstanceManager_OnNewInstance()
        {
            Application.Current?.Dispatcher.Invoke(BringToFront, DispatcherPriority.Background);
        }

        private void TabItem_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ((TabItem)sender).IsSelected = true;
        }

        private void TabItem_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
        }

        private void TabItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DataContext is ShellViewModel shellViewModel)
            {
                var workspaceViewModel = (sender as TabItem)?.DataContext as WorkspaceViewModel;
                shellViewModel.WorkspaceMenu.OpenInNewWindowCommand.Execute(workspaceViewModel);
            }
        }

        private void lvBarcodes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;
            if (listView.DataContext is WorkspaceViewModel workspaceViewModel)
            {
                workspaceViewModel.SelectedBarcodes = listView.SelectedItems
                    .Cast<BarcodeViewModel>()
                    .ToList();
            }
        }
    }
}