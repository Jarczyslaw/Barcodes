using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Barcodes.Core.ViewModels
{
    public class SelectionViewModel<T> : BaseInputViewModel
    {
        private ObservableCollection<T> items;
        private T selectedItem;
        private string displayMemberPath;

        public SelectionViewModel(string title, string contentHeader, string label,
            IEnumerable<T> items, T selectedItem, string displayMemberPath)
            : base(title, contentHeader, label)
        {
            Items = new ObservableCollection<T>(items);
            SelectedItem = selectedItem;
            DisplayMemberPath = displayMemberPath;
        }

        public T Result { get; private set; }

        public ObservableCollection<T> Items
        {
            get => items;
            set => SetProperty(ref items, value);
        }

        public T SelectedItem
        {
            get => selectedItem;
            set => SetProperty(ref selectedItem, value);
        }

        public string DisplayMemberPath
        {
            get => displayMemberPath;
            set => SetProperty(ref displayMemberPath, value);
        }

        protected override void Accept()
        {
            Result = SelectedItem;
        }
    }
}
