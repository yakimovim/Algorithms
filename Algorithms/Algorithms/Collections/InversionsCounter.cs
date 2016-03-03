using System;
using System.Collections.Generic;
using System.Linq;

namespace EdlinSoftware.Algorithms.Collections
{
    /// <summary>
    /// Represents counter of invertions in an array.
    /// For an array A invertion is a pair of indexes (i,j) such that i &lt; j and A[i] &gt; A[j].
    /// </summary>
    public static class InversionsCounter
    {
        private class Holder<T>
        {
            public T[] SortedArray;
            public long InversionsCount;
        }

        /// <summary>
        /// Returns number of invertions in the given array.
        /// </summary>
        /// <typeparam name="T">Type of elements of the array.</typeparam>
        /// <param name="array">Array of elements</param>
        /// <returns>Number of invertions in the <paramref name="array"/>.</returns>
        public static long Count<T>(IList<T> array)
            where T : IComparable<T>
        {
            return Count(array, Comparer<T>.Default);
        }

        /// <summary>
        /// Returns number of invertions in the given array.
        /// </summary>
        /// <typeparam name="T">Type of elements of the array.</typeparam>
        /// <param name="array">Array of elements</param>
        /// <param name="comparer">Comparer of elements of the <paramref name="array"/>.</param>
        /// <returns>Number of invertions in the <paramref name="array"/>.</returns>
        public static long Count<T>(IList<T> array, IComparer<T> comparer)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));

            var holder = SortAndCount(array, comparer);

            return holder.InversionsCount;
        }

        private static Holder<T> SortAndCount<T>(IList<T> array, IComparer<T> comparer)
        {
            if (array.Count < 2)
            {
                return new Holder<T>
                {
                    SortedArray = array.ToArray(),
                    InversionsCount = 0
                };
            }

            T[] left, right;
            SplitArray(array, out left, out right);

            var leftHolder = SortAndCount(left, comparer);
            var rightHolder = SortAndCount(right, comparer);
            var splitHolder = MergeAndCountSplitInversions(leftHolder.SortedArray, rightHolder.SortedArray, comparer);

            return new Holder<T>
            {
                SortedArray = splitHolder.SortedArray,
                InversionsCount = splitHolder.InversionsCount + leftHolder.InversionsCount + rightHolder.InversionsCount
            };
        }

        private static void SplitArray<T>(IList<T> unsorted, out T[] left, out T[] right)
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
                    left[i] = unsorted[k];
                    i++;
                }
                else
                {
                    right[j] = unsorted[k];
                    j++;
                }
            }
        }

        private static int Divide(int value)
        {
            if (value < 2)
                throw new ArgumentOutOfRangeException(nameof(value));

            return value / 2;
        }

        private static Holder<T> MergeAndCountSplitInversions<T>(T[] first, T[] second, IComparer<T> comparer)
        {
            var merged = new T[first.Length + second.Length];
            long inversionsCount = 0;

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

                    inversionsCount += (first.Length - i);
                }
            }

            return new Holder<T>
            {
                SortedArray = merged,
                InversionsCount = inversionsCount
            };
        }

        private static bool ShouldTakeElementFromFirstArray<T>(T[] first, T[] second, int firstIndex, int secondIndex, IComparer<T> comparer)
        {
            if (firstIndex >= first.Length)
                return false;
            if (secondIndex >= second.Length)
                return true;

            return comparer.Compare(first[firstIndex], second[secondIndex]) < 0;
        }
    }
}
