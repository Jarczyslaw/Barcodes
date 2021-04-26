using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;

namespace JToolbox.Desktop.Dialogs.Builders
{
    public class CommonSaveDialogBuilder : CommonDialogBuilderBase<CommonSaveFileDialog>
    {
        public CommonSaveDialogBuilder()
        {
            Dialog = new CommonSaveFileDialog()
            {
                IsExpandedMode = false,
                EnsureValidNames = true,
                AlwaysAppendDefaultExtension = true,
                RestoreDirectory = true,
            };
        }

        public CommonSaveDialogBuilder Initialize(string title, string initialDirectory)
        {
            Dialog.Title = title;
            Dialog.InitialDirectory = initialDirectory;
            return this;
        }

        public CommonSaveDialogBuilder SetDefaults(string defaultFileName, string defaultExtension)
        {
            Dialog.DefaultFileName = defaultFileName;
            Dialog.DefaultExtension = defaultExtension;
            return this;
        }

        public new CommonSaveDialogBuilder AddFilter(DialogFilterPair filter)
        {
            base.AddFilter(filter);
            return this;
        }

        public new CommonSaveDialogBuilder AddFilters(List<DialogFilterPair> filters)
        {
            base.AddFilters(filters);
            return this;
        }
    }
}
