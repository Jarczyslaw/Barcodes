using Microsoft.WindowsAPICodePack.Dialogs;
using System;

namespace JToolbox.Desktop.Dialogs.Builders
{
    public class TaskDialogBuilder
    {
        public TaskDialog Dialog { get; protected set; } = new TaskDialog
        {
            Cancelable = false
        };

        public TaskDialogBuilder Initialize(string caption, string text, TaskDialogStandardIcon icon, string instructionText = null)
        {
            Dialog.Caption = caption;
            Dialog.Text = text;
            Dialog.Icon = icon;
            Dialog.InstructionText = instructionText;
            return this;
        }

        public TaskDialogBuilder SetOwner(IntPtr handle)
        {
            Dialog.OwnerWindowHandle = handle;
            return this;
        }

        public TaskDialogBuilder AddDetails(string collapsedLabel, string expandedLabel, string text, bool expanded = false)
        {
            Dialog.DetailsExpanded = expanded;
            Dialog.DetailsCollapsedLabel = collapsedLabel;
            Dialog.DetailsExpandedLabel = expandedLabel;
            Dialog.DetailsExpandedText = text;
            return this;
        }

        public TaskDialogBuilder AddFooter(string text, TaskDialogStandardIcon icon)
        {
            Dialog.FooterIcon = icon;
            Dialog.FooterText = text;
            return this;
        }

        public TaskDialogBuilder AddFooterTextbox(string text, bool checkedState)
        {
            Dialog.FooterCheckBoxText = text;
            Dialog.FooterCheckBoxChecked = checkedState;
            return this;
        }

        public TaskDialogBuilder SetButtons(params TaskDialogStandardButtons[] buttons)
        {
            var buttonsSum = TaskDialogStandardButtons.None;
            foreach (var button in buttons)
            {
                buttonsSum |= button;
            }

            Dialog.StandardButtons = buttonsSum;
            return this;
        }

        public TaskDialogBuilder SetDefaultButton(TaskDialogDefaultButton defaultButton)
        {
            Dialog.DefaultButton = defaultButton;
            return this;
        }

        public TaskDialogBuilder AddCustomButton(string name, string text, EventHandler handler, bool setAsDefault = false)
        {
            var customButton = new TaskDialogButton(name, text);
            customButton.Click += handler;
            customButton.Default = setAsDefault;
            Dialog.Controls.Add(customButton);

            return this;
        }

        public TaskDialogBuilder AddCommandLink(string name, string text, string instruction, EventHandler handler, bool setAsDefault = false)
        {
            var commandLink = new TaskDialogCommandLink(name, text, instruction)
            {
                Default = setAsDefault
            };
            commandLink.Click += handler;
            Dialog.Controls.Add(commandLink);

            return this;
        }

        public TaskDialogBuilder AddProgressbar(int minValue, int maxValue, TaskDialogProgressBarState state)
        {
            Dialog.ProgressBar = new TaskDialogProgressBar(minValue, maxValue, minValue)
            {
                State = state
            };
            return this;
        }

        private void CheckDialogInstance()
        {
            if (Dialog == null)
            {
                throw new Exception(Resources.Resources.InitializeException);
            }
        }
    }
}
