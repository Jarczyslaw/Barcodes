using System;

namespace Barcodes.Core.ViewModels
{
    public class InputViewModel : BaseInputViewModel
    {
        private readonly Func<string, bool> validationRule;
        private string inputValue;

        public InputViewModel(string title, string contentHeader, string label, string inputValue, Func<string, bool> validationRule)
            : base(title, contentHeader, label)
        {
            this.inputValue = inputValue;
            this.validationRule = validationRule;
        }

        public string Result { get; private set; }

        public string InputValue
        {
            get => inputValue;
            set => SetProperty(ref inputValue, value);
        }

        protected override void Accept()
        {
            if (validationRule(InputValue))
            {
                Result = InputValue.Trim();
                OnClose?.Invoke();
            }
        }
    }
}
