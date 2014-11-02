using System;
using System.Collections.Generic;

namespace EdlinSoftware.Algorithms.Collections.Sorting
{
    /// <summary>
    /// Represents sorter of array using Quick algorithm.
    /// </summary>
    public static class QuickSorter
    {
        public static QuickSorter<T> New<T>(Func<IList<T>, int, int, int> pivotIndexSelector)
            where T : IComparable<T>
        {
            if (pivotIndexSelector == null) throw new ArgumentNullException("pivotIndexSelector");

            return new QuickSorter<T>(pivotIndexSelector, Comparer<T>.Default);
        }
    }

    /// <summary>
    /// Represents sorter of array using Quick algorithm.
    /// </summary>
    public class QuickSorter<T>
    {
        private readonly Func<IList<T>, int, int, int> _pivotIndexSelector;
        private readonly Partitioner<T> _partitioner;

        public QuickSorter(Func<IList<T>, int, int, int> pivotIndexSelector, IComparer<T> comparer)
        {
            if (pivotIndexSelector == null) throw new ArgumentNullException("pivotIndexSelector");
            if (comparer == null) throw new ArgumentNullException("comparer");
            _pivotIndexSelector = pivotIndexSelector;
            _partitioner = new Partitioner<T>(comparer);
        }

        public void Sort(IList<T> array)
        {
            if (array == null) throw new ArgumentNullException("array");

            InternalSort(array, 0, array.Count - 1);
        }

        private void InternalSort(IList<T> array, int left, int right)
        {
            if (array.Count < 2) return;

            if (left < 0 || left >= array.Count) throw new ArgumentOutOfRangeException("left", "Index is out of array");
            if (right < 0 || right >= array.Count) throw new ArgumentOutOfRangeException("right", "Index is out of array");
            if (left > right) throw new ArgumentOutOfRangeException("right", "Left index should be smaller then right");

            if (right - left == 0) return;

            var pivotIndex = _pivotIndexSelector(array, left, right);

            var newPivotIndex = _partitioner.Partition(array, left, right, pivotIndex);

            if (newPivotIndex > left)
            {
                InternalSort(array, left, newPivotIndex - 1);
            }
            if (newPivotIndex < right)
            {
                InternalSort(array, newPivotIndex + 1, right);
            }
        }
    }
}
