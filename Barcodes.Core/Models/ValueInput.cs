using System;

namespace Barcodes.Core.Models
{
    public class ValueInput : BaseInput
    {
        public string InputValue { get; set; }
        public Func<string, bool> ValidationRule { get; set; }
    }
}
