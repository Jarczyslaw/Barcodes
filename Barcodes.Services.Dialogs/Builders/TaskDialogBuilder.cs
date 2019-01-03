using Microsoft.WindowsAPICodePack.Dialogs;
using System;

namespace Barcodes.Services.Dialogs.Builders
{
    public class TaskDialogBuilder
    {
        private TaskDialog dialog;

        public TaskDialogBuilder Initialize(string caption, string text, TaskDialogStandardIcon icon, string instructionText = null)
        {
            dialog = new TaskDialog
            {
                Caption = caption,
                Text = text,
                Icon = icon,
                InstructionText = instructionText,
                Cancelable = false
            };
            return this;
        }

        public TaskDialogBuilder SetOwner(IntPtr handle)
        {
            CheckDialogInstance();

            dialog.OwnerWindowHandle = handle;
            return this;
        }

        public TaskDialogBuilder AddDetails(string collapsedLabel, string expandedLabel, string text, bool expanded = false)
        {
            CheckDialogInstance();

            dialog.DetailsExpanded = expanded;
            dialog.DetailsCollapsedLabel = collapsedLabel;
            dialog.DetailsExpandedLabel = expandedLabel;
            dialog.DetailsExpandedText = text;
            return this;
        }

        public TaskDialogBuilder AddFooter(string text, TaskDialogStandardIcon icon)
        {
            CheckDialogInstance();

            dialog.FooterIcon = icon;
            dialog.FooterText = text;
            return this;
        }

        public TaskDialogBuilder AddFooterTextbox(string text, bool checkedState)
        {
            CheckDialogInstance();

            dialog.FooterCheckBoxText = text;
            dialog.FooterCheckBoxChecked = checkedState;
            return this;
        }

        public TaskDialogBuilder SetButtons(params TaskDialogStandardButtons[] buttons)
        {
            CheckDialogInstance();

            var buttonsSum = TaskDialogStandardButtons.None;
            foreach (var button in buttons)
            {
                buttonsSum |= button;
            }

            dialog.StandardButtons = buttonsSum;
            return this;
        }

        public TaskDialogBuilder SetDefaultButton(TaskDialogDefaultButton defaultButton)
        {
            CheckDialogInstance();

            dialog.DefaultButton = defaultButton;
            return this;
        }

        public TaskDialogBuilder AddCustomButton(string name, string text, EventHandler handler, bool setAsDefault = false)
        {
            CheckDialogInstance();

            var customButton = new TaskDialogButton(name, text);
            customButton.Click += handler;
            customButton.Default = setAsDefault;
            dialog.Controls.Add(customButton);
            return this;
        }

        public TaskDialogBuilder AddCommandLink(string name, string text, string instruction, EventHandler handler, bool setAsDefault = false)
        {
            CheckDialogInstance();

            var commandLink = new TaskDialogCommandLink(name, text, instruction)
            {
                Default = setAsDefault
            };
            commandLink.Click += handler;
            dialog.Controls.Add(commandLink);
            return this;
        }

        public TaskDialogBuilder AddProgressbar(int minValue, int maxValue, TaskDialogProgressBarState state)
        {
            CheckDialogInstance();

            dialog.ProgressBar = new TaskDialogProgressBar(minValue, maxValue, minValue)
            {
                State = state
            };
            return this;
        }

        private void CheckDialogInstance()
        {
            if (dialog == null)
            {
                throw new Exception(Resources.Resources.InitializeException);
            }
        }

        public TaskDialog Build()
        {
            CheckDialogInstance();
            return dialog;
        }
    }
}
