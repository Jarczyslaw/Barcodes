using System.Collections.Generic;

namespace Barcodes.Extensions
{
    public static class IListExtensions
    {
        public static List<int> IndexesOf<T>(this IList<T> @this, IList<T> entries)
        {
            var result = new List<int>();
            foreach (var entry in entries)
            {
                result.Add(@this.IndexOf(entry));
            }
            result.Sort();
            return result;
        }

        public static List<T> GetByIndexes<T>(this IList<T> @this, IList<int> indexes)
        {
            var result = new List<T>();
            foreach (var index in indexes)
            {
                result.Add(@this[index]);
            }
            return result;
        }

        public static IList<T> GetRange<T>(this IList<T> @this, int index, int count)
        {
            var result = new List<T>();
            for (int i = index; i < index + count; i++)
            {
                result.Add(@this[i]);
            }
            return result;
        }

        #region ShiftLeft

        public static bool ShiftLeft<T>(this IList<T> @this, int index)
        {
            if (@this.Count < 2 || index < 1)
                return false;

            var temp = @this[index - 1];
            @this.RemoveAt(index - 1);
            @this.Insert(index, temp);
            return true;
        }

        public static bool ShiftLeft<T>(this IList<T> @this, List<int> indexes, bool skipSorting = false)
        {
            var changed = false;
            if (!skipSorting)
            {
                indexes.Sort();
            }
            for (int i = 0; i < indexes.Count; i++)
            {
                var index = indexes[i];
                if (index > 0 && !indexes.Contains(index - 1))
                {
                    @this.ShiftLeft(index);
                    indexes[i]--;
                    changed = true;
                }
            }
            return changed;
        }

        public static void ShiftLeft<T>(this IList<T> @this, T entry)
        {
            ShiftLeft(@this, @this.IndexOf(entry));
        }

        public static void ShiftLeft<T>(this IList<T> @this, IList<T> entries)
        {
            ShiftLeft(@this, @this.IndexesOf(entries));
        }

        #endregion ShiftLeft

        #region ShiftRight

        public static bool ShiftRight<T>(this IList<T> @this, int index)
        {
            if (@this.Count < 2 || index > @this.Count - 2)
                return false;

            var temp = @this[index + 1];
            @this.RemoveAt(index + 1);
            @this.Insert(index, temp);
            return true;
        }

        public static bool ShiftRight<T>(this IList<T> @this, List<int> indexes, bool skipSorting = false)
        {
            var changed = false;
            if (!skipSorting)
            {
                indexes.Sort();
            }
            for (int i = indexes.Count - 1; i >= 0; i--)
            {
                var index = indexes[i];
                if (index < @this.Count - 1 && !indexes.Contains(index + 1))
                {
                    @this.ShiftRight(index);
                    indexes[i]++;
                    changed = true;
                }
            }
            return changed;
        }

        public static void ShiftRight<T>(this IList<T> @this, T entry)
        {
            ShiftRight(@this, @this.IndexOf(entry));
        }

        public static void ShiftRight<T>(this IList<T> @this, IList<T> entries)
        {
            ShiftRight(@this, @this.IndexesOf(entries));
        }

        #endregion ShiftRight

        #region Swap

        public static void Swap<T>(this IList<T> @this, T oldValue, T newValue)
        {
            var oldIndex = @this.IndexOf(oldValue);
            var newIndex = @this.IndexOf(newValue);
            Swap(@this, oldIndex, newIndex);
        }

        public static void Swap<T>(this IList<T> @this, int index1, int index2)
        {
            if (index1 >= 0 && index2 >= 0)
            {
                var temp = @this[index1];
                @this[index1] = @this[index2];
                @this[index2] = temp;
            }
        }

        #endregion Swap

        #region SetAsFirst

        public static void SetAsFirst<T>(this IList<T> @this, int index)
        {
            @this.SetAsFirst(new List<int> { index });
        }

        public static void SetAsFirst<T>(this IList<T> @this, T item)
        {
            @this.SetAsFirst(@this.IndexOf(item));
        }

        public static void SetAsFirst<T>(this IList<T> @this, List<T> items)
        {
            @this.SetAsFirst(@this.IndexesOf(items));
        }

        public static void SetAsFirst<T>(this IList<T> @this, List<int> indexes)
        {
            indexes.Sort();
            while (@this.ShiftLeft(indexes, true)) ;
        }

        #endregion SetAsFirst

        #region SetAsLast

        public static void SetAsLast<T>(this IList<T> @this, int index)
        {
            @this.SetAsLast(new List<int> { index });
        }

        public static void SetAsLast<T>(this IList<T> @this, T item)
        {
            @this.SetAsLast(@this.IndexOf(item));
        }

        public static void SetAsLast<T>(this IList<T> @this, List<T> items)
        {
            @this.SetAsLast(@this.IndexesOf(items));
        }

        public static void SetAsLast<T>(this IList<T> @this, List<int> indexes)
        {
            indexes.Sort();
            while (@this.ShiftRight(indexes, true)) ;
        }

        #endregion SetAsLast
    }
}