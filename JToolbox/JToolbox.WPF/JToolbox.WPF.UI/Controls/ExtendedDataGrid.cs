using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace JToolbox.WPF.UI.Controls
{
    public class ExtendedDataGrid<T> : DataGrid
    {
        public static readonly DependencyProperty SelectedItemsExProperty = DependencyProperty.Register("SelectedItemsEx", typeof(ObservableCollection<T>), typeof(ExtendedDataGrid<T>),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(SelectedItemsExChanged)));

        private bool updateFromSelectionChanged;

        public ExtendedDataGrid()
        {
            AutoGenerateColumns =
                CanUserAddRows =
                CanUserDeleteRows =
                CanUserResizeRows = false;
            SelectionUnit = DataGridSelectionUnit.FullRow;
            SelectionMode = DataGridSelectionMode.Extended;
            RowHeaderWidth = 0;
            IsReadOnly = true;

            SelectionChanged += ExtendedDataGrid_SelectionChanged;
        }

        public ObservableCollection<T> SelectedItemsEx
        {
            get => (ObservableCollection<T>)GetValue(SelectedItemsExProperty);
            set => SetValue(SelectedItemsExProperty, value);
        }

        private void ExtendedDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateFromSelectionChanged = true;
            if (SelectedItems == null)
            {
                SelectedItemsEx = null;
            }
            else
            {
                var items = new ObservableCollection<T>();
                foreach (var item in SelectedItems)
                {
                    items.Add((T)item);
                }
                SelectedItemsEx = items;
            }
        }

        private static void SelectedItemsExChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            var grid = dependencyObject as ExtendedDataGrid<T>;
            var items = args.NewValue as IEnumerable;
            grid.SelectedItemsExChanged(items);
        }

        private void SelectedItemsExChanged(IEnumerable items)
        {
            if (updateFromSelectionChanged)
            {
                updateFromSelectionChanged = false;
                return;
            }

            if (items != null)
            {
                SelectionChanged -= ExtendedDataGrid_SelectionChanged;
                SelectedItems.Clear();
                foreach (var item in items)
                {
                    SelectedItems.Add(item);
                }
                SelectionChanged += ExtendedDataGrid_SelectionChanged;
            }
        }
    }
}