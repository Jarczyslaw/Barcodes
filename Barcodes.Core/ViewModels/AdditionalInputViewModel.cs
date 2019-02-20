using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Core.ViewModels
{
    public class AdditionalInputViewModel
    {
        public string Title { get; set; }

        public Func<string, string> Handler { get; set; }
    }
}
