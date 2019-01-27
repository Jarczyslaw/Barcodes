using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Core.Services.StateSaver.States
{
    public class Ean128ProductViewModelState
    {
        public string ProductCode { get; set; }
        public string SerialNumber { get; set; }
        public DateTime SelectedDateTime { get; set; }
        public bool EmptyDay { get; set; }
    }
}
