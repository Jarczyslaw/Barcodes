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

        public static void ShiftLeft<T>(this IList<T> @this, int index)
        {
            if (@this.Count < 2)
                return;

            if (index < 1)
                return;

            var temp = @this[index - 1];
            @this.RemoveAt(index - 1);
            @this.Insert(index, temp);
        }

        public static void ShiftLeft<T>(this IList<T> @this, List<int> indexes)
        {
            indexes.Sort();
            for (int i = 0; i < indexes.Count; i++)
            {
                var index = indexes[i];
                if (index > 0 && !indexes.Contains(index - 1))
                {
                    @this.ShiftLeft(index);
                    indexes[i]--;
                }
            }
        }

        public static void ShiftLeft<T>(this IList<T> @this, T entry)
        {
            ShiftLeft(@this, @this.IndexOf(entry));
        }

        public static void ShiftLeft<T>(this IList<T> @this, IList<T> entries)
        {
            ShiftLeft(@this, @this.IndexesOf(entries));
        }

        public static void ShiftRight<T>(this IList<T> @this, int index)
        {
            if (@this.Count < 2)
                return;

            if (index > @this.Count - 2)
                return;

            var temp = @this[index + 1];
            @this.RemoveAt(index + 1);
            @this.Insert(index, temp);
        }

        public static void ShiftRight<T>(this IList<T> @this, List<int> indexes)
        {
            indexes.Sort();
            for (int i = indexes.Count - 1; i >= 0; i--)
            {
                var index = indexes[i];
                if (index < @this.Count - 1 && !indexes.Contains(index + 1))
                {
                    @this.ShiftRight(index);
                    indexes[i]++;
                }
            }
        }

        public static void ShiftRight<T>(this IList<T> @this, T entry)
        {
            ShiftRight(@this, @this.IndexOf(entry));
        }

        public static void ShiftRight<T>(this IList<T> @this, IList<T> entries)
        {
            ShiftRight(@this, @this.IndexesOf(entries));
        }

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

        public static IList<T> GetRange<T>(this IList<T> @this, int index, int count)
        {
            var result = new List<T>();
            for (int i = index; i < index + count; i++)
            {
                result.Add(@this[i]);
            }
            return result;
        }

        public static bool SetAsFirst<T>(this IList<T> @this, int index)
        {
            if (index > 0)
            {
                var item = @this[index];
                @this.Remove(item);
                @this.Insert(0, item);
                return true;
            }
            return false;
        }

        public static void SetAsFirst<T>(this IList<T> @this, List<int> indexes)
        {
            indexes.Sort();
            indexes.Reverse();
            var items = new List<T>();
            foreach (var index in indexes)
            {
                items.Add(@this[index]);
                @this.RemoveAt(index);
            }

            foreach (var item in items)
            {
                @this.Insert(0, item);
            }
        }

        public static bool SetAsFirst<T>(this IList<T> @this, T item)
        {
            return SetAsFirst(@this, @this.IndexOf(item));
        }

        public static void SetAsFirst<T>(this IList<T> @this, List<T> items)
        {
            SetAsFirst(@this, IndexesOf(@this, items));
        }

        public static bool SetAsLast<T>(this IList<T> @this, int index)
        {
            if (index >= 0 && index < @this.Count - 1)
            {
                var item = @this[index];
                @this.Remove(item);
                @this.Add(item);
                return true;
            }
            return false;
        }

        public static bool SetAsLast<T>(this IList<T> @this, T item)
        {
            return SetAsLast(@this, @this.IndexOf(item));
        }

        public static void SetAsLast<T>(this IList<T> @this, List<int> indexes)
        {
            indexes.Sort();
            indexes.Reverse();
            var items = new List<T>();
            foreach (var index in indexes)
            {
                items.Add(@this[index]);
                @this.RemoveAt(index);
            }

            items.Reverse();
            foreach (var item in items)
            {
                @this.Add(item);
            }
        }

        public static void SetAsLast<T>(this IList<T> @this, List<T> items)
        {
            SetAsLast(@this, IndexesOf(@this, items));
        }
    }
}