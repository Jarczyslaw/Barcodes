using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Barcodes.Services.Dialogs.Builders
{
    public class CommonDialogBuilderBase<T>
        where T : CommonFileDialog
    {
        protected T dialog;

        protected void AddFilter(DialogFilterPair filter)
        {
            var dialogFilter = new CommonFileDialogFilter(filter.DisplayName, filter.ExtensionsList);
            dialog.DefaultExtension = filter.DisplayName;
            dialog.Filters.Add(dialogFilter);
        }

        protected void AddFilters(List<DialogFilterPair> filters)
        {
            if (filters != null)
            {
                dialog.DefaultExtension = filters[0].DisplayName;
                foreach (var filter in filters)
                {
                    AddFilter(filter);
                }
            }
        }

        protected void CheckDialogInstance()
        {
            if (dialog == null)
            {
                throw new Exception(Resources.Resources.InitializeException);
            }
        }

        public T Build()
        {
            CheckDialogInstance();
            return dialog;
        }
    }
}
