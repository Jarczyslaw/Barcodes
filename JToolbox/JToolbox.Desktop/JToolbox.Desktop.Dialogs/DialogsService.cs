using JToolbox.Desktop.Dialogs.Builders;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JToolbox.Desktop.Dialogs
{
    public class DialogsService : IDialogsService
    {
        public TaskDialogResult Show(TaskDialogBuilder builder)
        {
            using (var dialog = builder.Dialog)
            {
                return dialog.Show();
            }
        }

        public void ShowInfo(string message, string details = null, IntPtr? owner = null)
        {
            var builder = new TaskDialogBuilder()
                .Initialize(Resources.Resources.Information, message, TaskDialogStandardIcon.Information, Resources.Resources.Information)
                .AddDetails(Resources.Resources.ShowDetails, Resources.Resources.HideDetails, details)
                .SetOwner(GetOwnerHandle(owner));

            Show(builder);
        }

        public void ShowWarning(string message, string details = null, IntPtr? owner = null)
        {
            var builder = new TaskDialogBuilder()
                .Initialize(Resources.Resources.Warning, message, TaskDialogStandardIcon.Warning, Resources.Resources.Warning)
                .AddDetails(Resources.Resources.ShowDetails, Resources.Resources.HideDetails, details)
                .SetOwner(GetOwnerHandle(owner));

            Show(builder);
        }

        public void ShowError(string error, string details = null, IntPtr? owner = null)
        {
            var builder = new TaskDialogBuilder()
                .Initialize(Resources.Resources.Error, error, TaskDialogStandardIcon.Error, Resources.Resources.Error)
                .AddDetails(Resources.Resources.ShowDetails, Resources.Resources.HideDetails, details)
                .SetOwner(GetOwnerHandle(owner));

            Show(builder);
        }

        public void ShowException(Exception exception, string message = null, IntPtr? owner = null)
        {
            ShowExceptionDialog(Resources.Resources.Exception, exception, message, owner);
        }

        public void ShowCriticalException(Exception exception, string message = null, IntPtr? owner = null)
        {
            ShowExceptionDialog(Resources.Resources.CriticalException, exception, message, owner);
        }

        private void ShowExceptionDialog(string caption, Exception exception, string message, IntPtr? owner)
        {
            var text = exception.Message;
            if (!string.IsNullOrEmpty(message))
            {
                text = message + Environment.NewLine + text;
            }
            var builder = new TaskDialogBuilder()
                .Initialize(caption, text, TaskDialogStandardIcon.Error, Resources.Resources.ExceptionOccured)
                .AddDetails(Resources.Resources.ShowDetails, Resources.Resources.HideDetails, exception.StackTrace)
                .SetOwner(GetOwnerHandle(owner));

            Show(builder);
        }

        public bool ShowYesNoQuestion(string question, IntPtr? owner = null)
        {
            var builder = new TaskDialogBuilder()
                .Initialize(Resources.Resources.Question, question, TaskDialogStandardIcon.Information, Resources.Resources.Question)
                .SetButtons(TaskDialogStandardButtons.Yes, TaskDialogStandardButtons.No)
                .SetOwner(GetOwnerHandle(owner));

            return Show(builder) == TaskDialogResult.Yes;
        }

        public T ShowCustomButtonsQuestion<T>(string question, IEnumerable<CustomButtonData<T>> customButtons, IntPtr? owner = null)
        {
            var builder = new TaskDialogBuilder()
                .Initialize(Resources.Resources.Question, question, TaskDialogStandardIcon.Information, Resources.Resources.Question)
                .SetOwner(GetOwnerHandle(owner));

            var result = default(T);
            using (var dialog = builder.Dialog)
            {
                foreach (var customButton in customButtons)
                {
                    builder.AddCustomButton(customButton.Name, customButton.Caption, (s, e) =>
                    {
                        result = customButton.Value;
                        dialog.Close();
                    }, customButton.Default);
                }
                dialog.Show();
            }

            return result;
        }

        public void ShowProgressDialog(string caption, string text, string instruction, IntPtr? owner = null)
        {
            var builder = new TaskDialogBuilder()
                .Initialize(caption, text, TaskDialogStandardIcon.Information, instruction)
                .SetButtons(TaskDialogStandardButtons.Cancel)
                .AddProgressbar(0, 100, TaskDialogProgressBarState.Marquee)
                .SetOwner(GetOwnerHandle(owner));

            Show(builder);
        }

        public List<string> Open(CommonOpenDialogBuilder builder, IntPtr? owner = null)
        {
            List<string> result = null;
            using (var dialog = builder.Dialog)
            {
                if (dialog.ShowDialog(GetOwnerHandle(owner)) == CommonFileDialogResult.Ok)
                {
                    result = dialog.FileNames.ToList();
                }
            }
            return result;
        }

        private string OpenSingle(CommonOpenDialogBuilder builder, IntPtr? owner = null)
        {
            var files = Open(builder, owner);
            if (files?.Count == 1)
            {
                return files[0];
            }
            return null;
        }

        public string OpenFile(string title, string initialDirectory = null, List<DialogFilterPair> filters = null, IntPtr? owner = null)
        {
            var builder = new CommonOpenDialogBuilder()
                .Initialize(title, initialDirectory)
                .SetAsFileDialog(false)
                .AddFilters(filters);

            return OpenSingle(builder, owner);
        }

        public List<string> OpenFiles(string title, string initialDirectory = null, List<DialogFilterPair> filters = null, IntPtr? owner = null)
        {
            var builder = new CommonOpenDialogBuilder()
                .Initialize(title, initialDirectory)
                .SetAsFileDialog(true)
                .AddFilters(filters);

            return Open(builder, owner);
        }

        public string OpenFolder(string title, string initialDirectory = null, IntPtr? owner = null)
        {
            var builder = new CommonOpenDialogBuilder()
                .Initialize(title, initialDirectory)
                .SetAsFolderDialog();

            return OpenSingle(builder, owner);
        }

        public string Save(CommonSaveDialogBuilder builder, IntPtr? owner = null)
        {
            string result = null;
            using (var dialog = builder.Dialog)
            {
                if (dialog.ShowDialog(GetOwnerHandle(owner)) == CommonFileDialogResult.Ok)
                {
                    result = dialog.FileName;
                }
            }
            return result;
        }

        public string SaveFile(string title, string initialDirectory, string defaultFileName = null, DialogFilterPair filter = null, IntPtr? owner = null)
        {
            var builder = new CommonSaveDialogBuilder()
                .Initialize(title, initialDirectory)
                .SetDefaults(defaultFileName, filter.ExtensionsList)
                .AddFilter(filter);

            return Save(builder, owner);
        }

        private IntPtr GetOwnerHandle(IntPtr? owner)
        {
            if (owner == null)
            {
                return NativeMethods.GetActiveWindow();
            }
            else if (owner.Value == IntPtr.Zero)
            {
                return IntPtr.Zero;
            }
            else
            {
                return owner.Value;
            }
        }
    }
}