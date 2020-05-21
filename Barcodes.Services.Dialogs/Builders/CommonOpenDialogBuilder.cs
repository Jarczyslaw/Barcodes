using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;

namespace Barcodes.Services.Dialogs.Builders
{
    public class CommonOpenDialogBuilder : CommonDialogBuilderBase<CommonOpenFileDialog>
    {
        public CommonOpenDialogBuilder Initialize(string title, string initialDirectory)
        {
            dialog = new CommonOpenFileDialog
            {
                Title = title,
                RestoreDirectory = true,
                InitialDirectory = initialDirectory
            };
            return this;
        }

        public CommonOpenDialogBuilder SetAsFileDialog(bool multiselect, bool ensureFileExists)
        {
            CheckDialogInstance();

            dialog.IsFolderPicker = false;
            dialog.EnsureFileExists = ensureFileExists;
            dialog.Multiselect = multiselect;
            return this;
        }

        public CommonOpenDialogBuilder SetAsFolderDialog()
        {
            CheckDialogInstance();

            dialog.IsFolderPicker = true;
            dialog.EnsurePathExists = true;
            dialog.Multiselect = false;
            return this;
        }

        public new CommonOpenDialogBuilder AddFilter(DialogFilterPair filter)
        {
            CheckDialogInstance();

            base.AddFilter(filter);
            return this;
        }

        public new CommonOpenDialogBuilder AddFilters(List<DialogFilterPair> filters)
        {
            CheckDialogInstance();

            base.AddFilters(filters);
            return this;
        }
    }
}
