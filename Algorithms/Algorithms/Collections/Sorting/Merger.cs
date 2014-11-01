using System;
using System.Collections.Generic;

namespace EdlinSoftware.Algorithms.Collections.Sorting
{
    /// <summary>
    /// Represents merger of two sorted arrays.
    /// </summary>
    public class Merger
    {
        /// <summary>
        /// Merges two sorted arrays into one sorted array.
        /// </summary>
        /// <typeparam name="T">Type of array elements.</typeparam>
        /// <param name="first">First sorted array.</param>
        /// <param name="second">Second sorted array.</param>
        /// <returns>Sorted array containing elements from both arrays.</returns>
        public T[] Merge<T>(IList<T> first, IList<T> second)
                    where T : IComparable<T>
        {
            return Merge(first, second, Comparer<T>.Default);
        }

        /// <summary>
        /// Merges two sorted arrays into one sorted array.
        /// </summary>
        /// <typeparam name="T">Type of array elements.</typeparam>
        /// <param name="first">First sorted array.</param>
        /// <param name="second">Second sorted array.</param>
        /// <param name="comparer">Comparer of elements.</param>
        /// <returns>Sorted array containing elements from both arrays.</returns>
        public T[] Merge<T>(IList<T> first, IList<T> second, IComparer<T> comparer)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (comparer == null) throw new ArgumentNullException("comparer");

            var merged = new T[first.Count + second.Count];

            int i = 0;
            int j = 0;
            for (int k = 0; k < merged.Length; k++)
            {
                if (ShouldTakeElementFromFirstArray(first, second, i, j, comparer))
                {
                    merged[k] = first[i];
                    i++;
                }
                else
                {
                    merged[k] = second[j];
                    j++;
                }
            }

            return merged;
        }

        private static bool ShouldTakeElementFromFirstArray<T>(IList<T> first, IList<T> second, int firstIndex, int secondIndex, IComparer<T> comparer)
        {
            if (firstIndex >= first.Count)
                return false;
            if (secondIndex >= second.Count)
                return true;

            return comparer.Compare(first[firstIndex], second[secondIndex]) < 0;
        }
    }
}
