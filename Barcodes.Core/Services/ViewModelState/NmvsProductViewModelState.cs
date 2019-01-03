using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Core.Services.ViewModelState
{
    public class NmvsProductViewModelState
    {
        public string ProductCode { get; set; }
        public string BatchId { get; set; }
        public bool EmptyDay { get; set; }
        public DateTime SelectedDateTime { get; set; }
        public string SerialNo { get; set; }
    }
}
