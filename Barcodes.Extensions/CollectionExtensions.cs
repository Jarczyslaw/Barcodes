using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Extensions
{
    public static class CollectionExtensions
    {
        public static void ShiftLeft<T>(this Collection<T> list, int index)
        {
            if (list.Count < 2)
                return;

            if (index < 1)
                return;

            var temp = list[index - 1];
            list.RemoveAt(index - 1);
            list.Insert(index, temp);
        }

        public static void ShiftRight<T>(this Collection<T> list, int index)
        {
            if (list.Count < 2)
                return;

            if (index > list.Count - 2)
                return;

            var temp = list[index + 1];
            list.RemoveAt(index + 1);
            list.Insert(index, temp);
        }
    }
}
