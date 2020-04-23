using System.Collections.Generic;

namespace Barcodes.Core.Models
{
    public class SelectionInput<T> : BaseInput
    {
        public IEnumerable<T> Items { get; set; }
        public T SelectedItem { get; set; }
        public string DisplayMemberPath { get; set; }
    }
}
