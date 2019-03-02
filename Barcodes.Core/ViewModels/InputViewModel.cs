using Barcodes.Core.ViewModelsInput;
using System;

namespace Barcodes.Core.ViewModels
{
    public class InputViewModel : BaseInputViewModel
    {
        private readonly Func<string, bool> validationRule;
        private string inputValue;

        public InputViewModel(InputViewModelInput input)
            : base(input.Title, input.ContentHeader, input.Label)
        {
            inputValue = input.InputValue;
            validationRule = input.ValidationRule;
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
