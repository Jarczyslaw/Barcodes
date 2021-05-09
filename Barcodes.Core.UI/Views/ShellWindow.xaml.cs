using Barcodes.Core.Services;
using Barcodes.Core.ViewModels;
using Barcodes.Services.AppSettings;
using Barcodes.SingleInstance;
using JToolbox.WPF.UI.DragAndDrop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace Barcodes.Core.UI.Views
{
    public partial class ShellWindow : BaseWindow
    {
        private DragDropHelper dragDropHelper;
        private FileDragDropHelper fileDragDropHelper;

        public ShellWindow(SingleInstanceManager singleInstanceManager, ShellViewModel shellViewModel, IAppEvents appEvents, IAppSettingsService appSettingsService)
        {
            DataContext = shellViewModel;
            InitializeComponent();

            singleInstanceManager.OnNewInstance += SingleInstanceManager_OnNewInstance;
            appEvents.OnDragDropModeChanged += AppEvents_OnDragDropModeChanged;

            dragDropHelper = new DragDropHelper(tabControl, new List<DragDropPair>
            {
                new DragDropPair(typeof(ListViewItem)),
                new DragDropPair(typeof(ListViewItem), typeof(TabItem)),
                new DragDropPair(typeof(ListViewItem), typeof(ListView)),
                new DragDropPair(typeof(TabItem)),
                new DragDropPair(typeof(TabItem), typeof(TabPanel))
            });
            dragDropHelper.UnpinEvents();

            fileDragDropHelper = new FileDragDropHelper(tabControl, new List<Type>
            {
                typeof(ListViewItem),
                typeof(TabItem)
            }, new List<Type>
            {
                typeof(TabControl)
            });
            fileDragDropHelper.UnpinEvents();
            AppEvents_OnDragDropModeChanged(appSettingsService.DragDropMode);

            KeyDownHandlerEnabled = true;
        }

        private void AppEvents_OnDragDropModeChanged(DragDropMode mode)
        {
            dragDropHelper.UnpinEvents();
            fileDragDropHelper.UnpinEvents();
            if (mode == DragDropMode.Arrangement)
            {
                dragDropHelper.PinEvents();
            }
            else if (mode == DragDropMode.ImportExport)
            {
                fileDragDropHelper.PinEvents();
            }
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