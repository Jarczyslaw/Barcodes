using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Services.Dialogs
{
    public class CustomButtonData<T>
    {
        public T Value { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }
        public bool Default { get; set; }
    }
}
