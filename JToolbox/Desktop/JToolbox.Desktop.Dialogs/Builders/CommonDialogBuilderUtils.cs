using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialogs.Builders
{
    public class CommonDialogBuilderBase<T>
        where T : CommonFileDialog
    {
        protected T dialog;

        public CommonDialogBuilderBase<T> AddFilter(DialogFilterPair filter)
        {
            var dialogFilter = new CommonFileDialogFilter(filter.DisplayName, filter.ExtensionsList);
            dialog.Filters.Add(dialogFilter);
            return this;
        }

        public CommonDialogBuilderBase<T> AddFilters(List<DialogFilterPair> filters)
        {
            if (filters != null)
            {
                foreach (var filter in filters)
                    AddFilter(filter);
            }
            return this;
        }

        public T Build()
        {
            return dialog;
        }
    }
}
