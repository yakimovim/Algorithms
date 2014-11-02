using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EdlinSoftware.Algorithms.Collections.Sorting
{
    /// <summary>
    /// Represents partitioner of array.
    /// </summary>
    public static class Partitioner
    {
        public static Partitioner<T> New<T>()
            where T : IComparable<T>
        {
            return new Partitioner<T>(Comparer<T>.Default);
        }
    }

    /// <summary>
    /// Represents partitioner of array.
    /// </summary>
    /// <typeparam name="T">Type of array elements.</typeparam>
    public class Partitioner<T>
    {
        private readonly IComparer<T> _comparer;

        [DebuggerStepThrough]
        public Partitioner(IComparer<T> comparer)
        {
            if (comparer == null) throw new ArgumentNullException("comparer");
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
            if (array == null) throw new ArgumentNullException("array");
            if (left < 0 || left >= array.Count) throw new ArgumentOutOfRangeException("left", "Index is out of array");
            if (right < 0 || right >= array.Count) throw new ArgumentOutOfRangeException("right", "Index is out of array");
            if (left > right) throw new ArgumentOutOfRangeException("right", "Left index should be smaller then right");
            if (pivotIndex < left || pivotIndex > right) throw new ArgumentOutOfRangeException("pivotIndex", "Pivot index should be between left and right");

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
            if (array == null) throw new ArgumentNullException("array");
            if (from < 0 || from >= array.Count) throw new ArgumentOutOfRangeException("from", "Index is out of array");
            if (to < 0 || to >= array.Count) throw new ArgumentOutOfRangeException("to", "Index is out of array");
            if (from == to) return;

            T temp = array[from];
            array[from] = array[to];
            array[to] = temp;
        }
    }
}
