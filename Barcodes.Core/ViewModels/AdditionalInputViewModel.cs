using System;

namespace Barcodes.Core.ViewModels
{
    public class AdditionalInputViewModel
    {
        public string Title { get; set; }

        public Func<string, string> Handler { get; set; }
    }
}
