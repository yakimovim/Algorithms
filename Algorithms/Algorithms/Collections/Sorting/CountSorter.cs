using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace EdlinSoftware.Algorithms.Collections.Sorting
{
    /// <summary>
    /// Represents sorter of array using CountSort algorithm.
    /// </summary>
    public class CountSorter<T>
    {
        private readonly IComparer<T> _comparer;

        public CountSorter(IComparer<T> comparer = null)
        {
            _comparer = comparer ?? Comparer<T>.Default;
        }

        public T[] Sort([NotNull] IList<T> array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            var counts = new SortedDictionary<T, int>(_comparer);

            foreach (var value in array)
            {
                if (!counts.ContainsKey(value))
                    counts[value] = 1;
                else
                    counts[value]++;
            }

            var sortedValues = counts.Keys.ToArray();

            for (int i = 1; i < sortedValues.Length; i++)
            {
                counts[sortedValues[i]] += counts[sortedValues[i - 1]];
            }

            var sortedArray = new T[array.Count];

            for (int i = array.Count - 1; i >= 0; i--)
            {
                var value = array[i];

                sortedArray[counts[value] - 1] = value;

                counts[value]--;
            }

            return sortedArray;
        }
    }
}