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
            if (pivotIndexSelector == null) throw new ArgumentNullException(nameof(pivotIndexSelector));

            return new QuickSorter<T>(pivotIndexSelector, Comparer<T>.Default);
        }
    }

    /// <summary>
    /// Represents sorter of array using Quick algorithm.
    /// </summary>
    public class QuickSorter<T>
    {
        private readonly Func<IList<T>, int, int, int> _pivotIndexSelector;
        private readonly ThreeWayPartitioner<T> _partitioner;

        public QuickSorter(Func<IList<T>, int, int, int> pivotIndexSelector, IComparer<T> comparer)
        {
            if (pivotIndexSelector == null) throw new ArgumentNullException(nameof(pivotIndexSelector));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            _pivotIndexSelector = pivotIndexSelector;
            _partitioner = new ThreeWayPartitioner<T>(comparer);
        }

        public void Sort(IList<T> array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (array.Count < 2) return;

            InternalSort(array, 0, array.Count - 1);
        }

        private void InternalSort(IList<T> array, int left, int right)
        {
            if (right <= left) return;

            if (left < 0 || left >= array.Count) throw new ArgumentOutOfRangeException(nameof(left), "Index is out of array");
            if (right < 0 || right >= array.Count) throw new ArgumentOutOfRangeException(nameof(right), "Index is out of array");

            var pivotIndex = _pivotIndexSelector(array, left, right);

            var partitionPositions = _partitioner.Partition(array, left, right, pivotIndex);

            InternalSort(array, left, partitionPositions.Item1);
            InternalSort(array, partitionPositions.Item2, right);
        }
    }
}
