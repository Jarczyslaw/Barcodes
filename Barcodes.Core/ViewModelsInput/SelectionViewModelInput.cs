using System.Collections.Generic;

namespace Barcodes.Core.ViewModelsInput
{
    public class SelectionViewModelInput<T> : BaseInputViewModelInput
    {
        public IEnumerable<T> Items { get; set; }
        public T SelectedItem { get; set; }
        public string DisplayMemberPath { get; set; }
    }
}
