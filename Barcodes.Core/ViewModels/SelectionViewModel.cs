using Barcodes.Core.ViewModelsInput;
using System.Collections.ObjectModel;

namespace Barcodes.Core.ViewModels
{
    public class SelectionViewModel<T> : BaseInputViewModel
    {
        private ObservableCollection<T> items;
        private T selectedItem;
        private string displayMemberPath;

        public SelectionViewModel(SelectionViewModelInput<T> input)
            : base(input.Title, input.ContentHeader, input.Label)
        {
            Items = new ObservableCollection<T>(input.Items);
            SelectedItem = input.SelectedItem;
            DisplayMemberPath = input.DisplayMemberPath;
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
            OnClose?.Invoke();
        }
    }
}
