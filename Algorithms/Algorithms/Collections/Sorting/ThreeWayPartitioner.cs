using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EdlinSoftware.Algorithms.Collections.Sorting
{
    /// <summary>
    /// Represents three-way partitioner of array.
    /// </summary>
    public static class ThreeWayPartitioner
    {
        public static ThreeWayPartitioner<T> New<T>()
            where T : IComparable<T>
        {
            return new ThreeWayPartitioner<T>(Comparer<T>.Default);
        }
    }

    /// <summary>
    /// Represents three-way partitioner of array.
    /// </summary>
    /// <typeparam name="T">Type of array elements.</typeparam>
    public class ThreeWayPartitioner<T>
    {
        private readonly IComparer<T> _comparer;

        [DebuggerStepThrough]
        public ThreeWayPartitioner(IComparer<T> comparer)
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
        public Tuple<int, int> Partition(IList<T> array, int left, int right, int pivotIndex)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (left < 0 || left >= array.Count) throw new ArgumentOutOfRangeException(nameof(left), "Index is out of array");
            if (right < 0 || right >= array.Count) throw new ArgumentOutOfRangeException(nameof(right), "Index is out of array");
            if (left > right) throw new ArgumentOutOfRangeException(nameof(right), "Left index should be smaller then right");
            if (pivotIndex < left || pivotIndex > right) throw new ArgumentOutOfRangeException(nameof(pivotIndex), "Pivot index should be between left and right");

            Swap(array, left, pivotIndex);
            var pivotElement = array[left];

            /*
                * for low < index <= i1 values less then pivot
                * for i1 < index <= i2 values are equal to pivot
                * for i2 < index <= k values are greater then pivot
                * for index > k unknown
                */
            var i1 = left;
            var i2 = left;
            for (int k = left + 1; k <= right; k++)
            {
                var compareResult = _comparer.Compare(array[k], pivotElement);

                if (compareResult < 0)
                {
                    Swap(array, k, i2 + 1);
                    Swap(array, i2 + 1, i1 + 1);
                    i1++;
                    i2++;
                }
                else if (compareResult == 0)
                {
                    Swap(array, k, i2 + 1);
                    i2++;
                }
            }

            Swap(array, left, i1);

            return new Tuple<int, int>(i1 - 1, i2 + 1);
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
