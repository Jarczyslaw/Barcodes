using System;
using System.Collections.Generic;

namespace JToolbox.Core.Extensions
{
    public static class IListExtensions
    {
        private static Random random = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

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

        public static void ShiftLeftByIndex<T>(this IList<T> @this, int index)
        {
            if (@this.Count < 2)
                return;

            if (index < 1)
                return;

            var temp = @this[index - 1];
            @this.RemoveAt(index - 1);
            @this.Insert(index, temp);
        }

        public static void ShiftLeftByIndexes<T>(this IList<T> @this, List<int> indexes)
        {
            indexes.Sort();
            for (int i = 0; i < indexes.Count; i++)
            {
                var index = indexes[i];
                if (index > 0 && !indexes.Contains(index - 1))
                {
                    @this.ShiftLeftByIndex(index);
                    indexes[i]--;
                }
            }
        }

        public static void ShiftLeft<T>(this IList<T> @this, T entry)
        {
            ShiftLeftByIndex(@this, @this.IndexOf(entry));
        }

        public static void ShiftLeft<T>(this IList<T> @this, IList<T> entries)
        {
            ShiftLeftByIndexes(@this, @this.IndexesOf(entries));
        }

        #endregion ShiftLeft

        #region ShiftRight

        public static void ShiftRightByIndex<T>(this IList<T> @this, int index)
        {
            if (@this.Count < 2)
                return;

            if (index > @this.Count - 2)
                return;

            var temp = @this[index + 1];
            @this.RemoveAt(index + 1);
            @this.Insert(index, temp);
        }

        public static void ShiftRightByIndexes<T>(this IList<T> @this, List<int> indexes)
        {
            indexes.Sort();
            for (int i = indexes.Count - 1; i >= 0; i--)
            {
                var index = indexes[i];
                if (index < @this.Count - 1 && !indexes.Contains(index + 1))
                {
                    @this.ShiftRightByIndex(index);
                    indexes[i]++;
                }
            }
        }

        public static void ShiftRight<T>(this IList<T> @this, T entry)
        {
            ShiftRightByIndex(@this, @this.IndexOf(entry));
        }

        public static void ShiftRight<T>(this IList<T> @this, IList<T> entries)
        {
            ShiftRightByIndexes(@this, @this.IndexesOf(entries));
        }

        #endregion ShiftRight

        #region Swap

        public static void Swap<T>(this IList<T> @this, T value1, T value2)
        {
            var oldIndex = @this.IndexOf(value1);
            var newIndex = @this.IndexOf(value2);
            SwapByIndex(@this, oldIndex, newIndex);
        }

        public static void SwapByIndex<T>(this IList<T> @this, int index1, int index2)
        {
            if (index1 >= 0 && index2 >= 0)
            {
                var temp = @this[index1];
                @this[index1] = @this[index2];
                @this[index2] = temp;
            }
        }

        #endregion Swap

        #region SetAsFirstFast

        public static bool SetAsFirstFast<T>(this IList<T> @this, T item)
        {
            return SetAsFirstFastByIndex(@this, @this.IndexOf(item));
        }

        public static void SetAsFirstFast<T>(this IList<T> @this, List<T> items)
        {
            SetAsFirstFastByIndexes(@this, IndexesOf(@this, items));
        }

        public static bool SetAsFirstFastByIndex<T>(this IList<T> @this, int index)
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

        public static void SetAsFirstFastByIndexes<T>(this IList<T> @this, List<int> indexes)
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

        #endregion SetAsFirstFast

        #region SetAsLastFast

        public static bool SetAsLastFast<T>(this IList<T> @this, T item)
        {
            return SetAsLastFastByIndex(@this, @this.IndexOf(item));
        }

        public static void SetAsLastFast<T>(this IList<T> @this, List<T> items)
        {
            SetAsLastFastByIndexes(@this, IndexesOf(@this, items));
        }

        public static bool SetAsLastFastByIndex<T>(this IList<T> @this, int index)
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

        public static void SetAsLastFastByIndexes<T>(this IList<T> @this, List<int> indexes)
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

        #endregion SetAsLastFast

        #region SetAsFirst

        public static void SetAsFirstByIndex<T>(this IList<T> @this, int index)
        {
            @this.SetAsFirstByIndexes(new List<int> { index });
        }

        public static void SetAsFirst<T>(this IList<T> @this, T item)
        {
            @this.SetAsFirstByIndex(@this.IndexOf(item));
        }

        public static void SetAsFirst<T>(this IList<T> @this, List<T> items)
        {
            @this.SetAsFirstByIndexes(@this.IndexesOf(items));
        }

        public static void SetAsFirstByIndexes<T>(this IList<T> @this, List<int> indexes)
        {
            indexes.Sort();
            var targetIndex = 0;
            foreach (var index in indexes)
            {
                var temp = @this[index];
                for (int i = index; i > targetIndex; i--)
                {
                    @this[i] = @this[i - 1];
                }
                @this[targetIndex] = temp;
                targetIndex++;
            }
        }

        #endregion SetAsFirst

        #region SetAsLast

        public static void SetAsLastByIndex<T>(this IList<T> @this, int index)
        {
            @this.SetAsLastByIndexes(new List<int> { index });
        }

        public static void SetAsLast<T>(this IList<T> @this, T item)
        {
            @this.SetAsLastByIndex(@this.IndexOf(item));
        }

        public static void SetAsLast<T>(this IList<T> @this, List<T> items)
        {
            @this.SetAsLastByIndexes(@this.IndexesOf(items));
        }

        public static void SetAsLastByIndexes<T>(this IList<T> @this, List<int> indexes)
        {
            indexes.Sort();
            indexes.Reverse();
            var targetIndex = @this.Count - 1;
            foreach (var index in indexes)
            {
                var temp = @this[index];
                for (int i = index; i < targetIndex; i++)
                {
                    @this[i] = @this[i + 1];
                }
                @this[targetIndex] = temp;
                targetIndex--;
            }
        }

        #endregion SetAsLast

        #region Move

        public static void Move<T>(this IList<T> @this, T item, T target)
        {
            var sourceIndex = @this.IndexOf(item);
            var targetIndex = @this.IndexOf(target);
            @this.MoveByIndex(sourceIndex, targetIndex);
        }

        public static void MoveByIndex<T>(this IList<T> @this, int sourceIndex, int targetIndex)
        {
            var item = @this[targetIndex];
            @this.RemoveAt(targetIndex);
            @this.Insert(sourceIndex, item);
        }

        #endregion Move
    }
}