using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Utils
{
    public static class ListExtensions
    {
        public static void ShiftLeft<T>(this List<T> list, int index)
        {
            if (list.Count < 2)
                return;

            if (index < 1)
                return;

            var temp = list[index - 1];
            list[index - 1] = list[index];
            list[index - 1] = temp;
        }

        public static void ShiftRight<T>(this List<T> list, int index)
        {
            if (list.Count < 2)
                return;

            if (index > list.Count - 2)
                return;

            var temp = list[index + 1];
            list[index + 1] = list[index];
            list[index + 1] = temp;
        }
    }
}
