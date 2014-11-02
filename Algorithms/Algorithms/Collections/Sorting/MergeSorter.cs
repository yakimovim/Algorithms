using System;
using System.Collections.Generic;
using System.Linq;

namespace EdlinSoftware.Algorithms.Collections.Sorting
{
    /// <summary>
    /// Represents sorter of array using Merge algorithm.
    /// </summary>
    public class MergeSorter
    {
        /// <summary>
        /// Sorts input array.
        /// </summary>
        /// <typeparam name="T">Type of elements of array.</typeparam>
        /// <param name="unsorted">Unsorted array.</param>
        /// <returns>Sorted array.</returns>
        public T[] Sort<T>(IList<T> unsorted)
            where T : IComparable<T>
        {
            return Sort(unsorted, Comparer<T>.Default);
        }

        /// <summary>
        /// Sorts input array.
        /// </summary>
        /// <typeparam name="T">Type of elements of array.</typeparam>
        /// <param name="unsorted">Unsorted array.</param>
        /// <param name="comparer">Comparer of elements.</param>
        /// <returns>Sorted array.</returns>
        public T[] Sort<T>(IList<T> unsorted, IComparer<T> comparer)
        {
            if (unsorted == null) throw new ArgumentNullException("unsorted");
            if (comparer == null) throw new ArgumentNullException("comparer");

            return InternalSort(unsorted, comparer);
        }

        private T[] InternalSort<T>(IList<T> unsorted, IComparer<T> comparer)
        {
            if (unsorted.Count < 2)
            { return unsorted.ToArray(); }

            T[] left, right;
            SeparateArray(unsorted, out left, out right);

            var sortedLeft = InternalSort(left, comparer);
            var sortedRight = InternalSort(right, comparer);

            return Merger.Merge(sortedLeft, sortedRight, comparer);
        }

        private void SeparateArray<T>(IList<T> unsorted, out T[] left, out T[] right)
        {
            var leftSize = Divide(unsorted.Count);

            left = new T[leftSize];
            right = new T[unsorted.Count - leftSize];

            int i = 0;
            int j = 0;
            for (int k = 0; k < unsorted.Count; k++)
            {
                if (k < leftSize)
                {
                    left[i++] = unsorted[k];
                }
                else
                {
                    right[j++] = unsorted[k];
                }
            }
        }

        private static int Divide(int value)
        {
            if (value < 2)
            { throw new ArgumentOutOfRangeException("value"); }

            return value / 2;
        }
    }
}
