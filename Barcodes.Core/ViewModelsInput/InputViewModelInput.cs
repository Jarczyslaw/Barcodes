using System;

namespace Barcodes.Core.ViewModelsInput
{
    public class InputViewModelInput : BaseInputViewModelInput
    {
        public string InputValue { get; set; }
        public Func<string, bool> ValidationRule { get; set; }
    }
}
