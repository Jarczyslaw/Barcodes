using Barcodes.Codes;
using Barcodes.Core.Models;
using System;

namespace Barcodes.Core.ViewModels
{
    public class TemplateViewModel
    {
        public BarcodeTemplate? Template { get; set; }

        public string Title { get; set; }

        public Func<string, TemplateResult> Handler { get; set; }
    }
}