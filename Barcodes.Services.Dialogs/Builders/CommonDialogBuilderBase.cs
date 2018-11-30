using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Services.Dialogs.Builders
{
    public class CommonDialogBuilderBase<T>
        where T : CommonFileDialog
    {
        protected T dialog;

        protected void AddFilter(DialogFilterPair filter)
        {
            var dialogFilter = new CommonFileDialogFilter(filter.DisplayName, filter.ExtensionsList);
            dialog.Filters.Add(dialogFilter);
        }

        protected void AddFilters(List<DialogFilterPair> filters)
        {
            if (filters != null)
            {
                foreach (var filter in filters)
                    AddFilter(filter);
            }
        }

        protected void CheckDialogInstance()
        {
            if (dialog == null)
                throw new Exception(Resources.Resources.InitializeException);
        }

        public T Build()
        {
            CheckDialogInstance();
            return dialog;
        }
    }
}
