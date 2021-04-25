using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;

namespace JToolbox.Desktop.Dialogs.Builders
{
    public class CommonDialogBuilderBase<T>
        where T : CommonFileDialog
    {
        public T Dialog { get; protected set; }

        protected void AddFilter(DialogFilterPair filter)
        {
            var dialogFilter = new CommonFileDialogFilter(filter.DisplayName, filter.ExtensionsList);
            Dialog.Filters.Add(dialogFilter);
        }

        protected void AddFilters(List<DialogFilterPair> filters)
        {
            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    AddFilter(filter);
                }
            }
        }
    }
}
