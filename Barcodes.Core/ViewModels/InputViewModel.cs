using Barcodes.Core.Common;
using Prism.Commands;
using Prism.Mvvm;
using System;

namespace Barcodes.Core.ViewModels
{
    public class InputViewModel : BindableBase, ICloseAware
    {
        private string title;
        private string contentHeader;
        private string label;
        private readonly Func<string, bool> validationRule;
        private string inputValue;

        public InputViewModel(string title, string contentHeader, string label, string inputValue, Func<string, bool> validationRule)
        {
            this.title = title;
            this.contentHeader = contentHeader;
            this.label = label;
            this.inputValue = inputValue;
            this.validationRule = validationRule;

            AcceptCommand = new DelegateCommand(() =>
            {
                if (validationRule(InputValue))
                {
                    Result = InputValue;
                    OnClose?.Invoke();
                }
            });
            CloseCommand = new DelegateCommand(() => OnClose?.Invoke());
        }

        public DelegateCommand AcceptCommand { get; }
        public DelegateCommand CloseCommand { get; }

        public Action OnClose { get; set; }

        public string Result { get; private set; }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public string ContentHeader
        {
            get => contentHeader;
            set => SetProperty(ref contentHeader, value);
        }

        public string Label
        {
            get => label;
            set => SetProperty(ref label, value);
        }

        public string InputValue
        {
            get => inputValue;
            set => SetProperty(ref inputValue, value);
        }
    }
}
