using System.Windows;
using System.Windows.Controls;

namespace JToolbox.WPF.UI.Controls
{
    public class ExtendedTreeView<T> : TreeView
    {
        public static readonly DependencyProperty SelectedItemExProperty = DependencyProperty.Register("SelectedItemEx", typeof(T), typeof(ExtendedTreeView<T>),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(SelectedItemsExChanged)));

        public ExtendedTreeView()
        {
            SelectedItemChanged += ExtendedTreeView_SelectedItemChanged;
        }

        public T SelectedItemEx
        {
            get => (T)GetValue(SelectedItemExProperty);
            set => SetValue(SelectedItemExProperty, value);
        }

        private void ExtendedTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItemEx = (T)e.NewValue;
        }

        private static void SelectedItemsExChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
        }
    }
}