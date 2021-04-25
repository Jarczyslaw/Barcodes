using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;

namespace JToolbox.Desktop.Dialogs.Builders
{
    public class CommonOpenDialogBuilder : CommonDialogBuilderBase<CommonOpenFileDialog>
    {
        public CommonOpenDialogBuilder()
        {
            Dialog = new CommonOpenFileDialog
            {
                RestoreDirectory = true,
                EnsureValidNames = true,
            };
        }

        public CommonOpenDialogBuilder Initialize(string title, string initialDirectory)
        {
            Dialog.Title = title;
            Dialog.InitialDirectory = initialDirectory;
            return this;
        }

        public CommonOpenDialogBuilder SetAsFileDialog(bool multiselect)
        {
            Dialog.IsFolderPicker = false;
            Dialog.EnsureFileExists = true;
            Dialog.Multiselect = multiselect;
            return this;
        }

        public CommonOpenDialogBuilder SetAsFolderDialog()
        {
            Dialog.IsFolderPicker = true;
            Dialog.EnsurePathExists = true;
            Dialog.Multiselect = false;
            return this;
        }

        public CommonOpenDialogBuilder SetFileNames()
        {
            return this;
        }

        public new CommonOpenDialogBuilder AddFilter(DialogFilterPair filter)
        {
            base.AddFilter(filter);
            return this;
        }

        public new CommonOpenDialogBuilder AddFilters(List<DialogFilterPair> filters)
        {
            base.AddFilters(filters);
            return this;
        }
    }
}