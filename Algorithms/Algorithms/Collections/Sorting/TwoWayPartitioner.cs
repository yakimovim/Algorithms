using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EdlinSoftware.Algorithms.Collections.Sorting
{
    /// <summary>
    /// Represents two-way partitioner of array.
    /// </summary>
    public static class TwoWayPartitioner
    {
        public static TwoWayPartitioner<T> New<T>()
            where T : IComparable<T>
        {
            return new TwoWayPartitioner<T>(Comparer<T>.Default);
        }
    }

    /// <summary>
    /// Represents two-way partitioner of array.
    /// </summary>
    /// <typeparam name="T">Type of array elements.</typeparam>
    public class TwoWayPartitioner<T>
    {
        private readonly IComparer<T> _comparer;

        [DebuggerStepThrough]
        public TwoWayPartitioner(IComparer<T> comparer)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            _comparer = comparer;
        }

        /// <summary>
        /// Partitions the array.
        /// </summary>
        /// <param name="array">Array.</param>
        /// <param name="left">Left border of partitioning.</param>
        /// <param name="right">Right border of partitioning.</param>
        /// <param name="pivotIndex">Pivot index.</param>
        /// <returns>Array with partitioned part.</returns>
        public int Partition(IList<T> array, int left, int right, int pivotIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (left < 0 || left >= array.Count) throw new ArgumentOutOfRangeException(nameof(left), "Index is out of array");
            if (right < 0 || right >= array.Count) throw new ArgumentOutOfRangeException(nameof(right), "Index is out of array");
            if (left > right) throw new ArgumentOutOfRangeException(nameof(right), "Left index should be smaller then right");
            if (pivotIndex < left || pivotIndex > right) throw new ArgumentOutOfRangeException(nameof(pivotIndex), "Pivot index should be between left and right");

            if (left == right) return pivotIndex;

            T pivotElement = array[pivotIndex];

            Swap(array, left, pivotIndex);

            int i = left;
            for (int j = left + 1; j <= right; j++)
            {
                if (_comparer.Compare(array[j], pivotElement) <= 0)
                {
                    Swap(array, i + 1, j);
                    i++;
                }
            }

            Swap(array, left, i);

            return i;
        }

        private void Swap(IList<T> array, int from, int to)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (from < 0 || from >= array.Count) throw new ArgumentOutOfRangeException(nameof(@from), "Index is out of array");
            if (to < 0 || to >= array.Count) throw new ArgumentOutOfRangeException(nameof(to), "Index is out of array");
            if (from == to) return;

            T temp = array[from];
            array[from] = array[to];
            array[to] = temp;
        }
    }
}
