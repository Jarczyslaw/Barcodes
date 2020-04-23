using Barcodes.Core.Models;
using System;

namespace Barcodes.Core.ViewModels
{
    public class TemplateViewModel
    {
        public string Title { get; set; }

        public Func<string, TemplateResult> Handler { get; set; }
    }
}
