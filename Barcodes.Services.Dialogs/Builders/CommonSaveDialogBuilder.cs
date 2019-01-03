using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;

namespace Barcodes.Services.Dialogs.Builders
{
    public class CommonSaveDialogBuilder : CommonDialogBuilderBase<CommonSaveFileDialog>
    {
        public CommonSaveDialogBuilder Initialize(string title, string initialDirectory)
        {
            dialog = new CommonSaveFileDialog()
            {
                Title = title,
                IsExpandedMode = false,
                EnsureValidNames = true,
                AlwaysAppendDefaultExtension = true,
                RestoreDirectory = true,
                InitialDirectory = initialDirectory
            };
            return this;
        }

        public CommonSaveDialogBuilder SetDefaults(string defaultFileName, string defaultExtension)
        {
            CheckDialogInstance();

            dialog.DefaultFileName = defaultFileName;
            dialog.DefaultExtension = defaultExtension;
            return this;
        }

        public new CommonSaveDialogBuilder AddFilter(DialogFilterPair filter)
        {
            CheckDialogInstance();

            base.AddFilter(filter);
            return this;
        }

        public new CommonSaveDialogBuilder AddFilters(List<DialogFilterPair> filters)
        {
            CheckDialogInstance();

            base.AddFilters(filters);
            return this;
        }
    }
}
