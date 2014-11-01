using System;
using System.Collections.Generic;
using System.Linq;

namespace EdlinSoftware.Algorithms.Graphs
{
    public static class OptimalSearchTreeBuilder
    {
        public static OptimalSearchTreeBuilder<TValue> New<TValue>(Func<TValue, double> searchFrequencyProvider)
            where TValue : IComparable<TValue>
        {
            return new OptimalSearchTreeBuilder<TValue>(Comparer<TValue>.Default, searchFrequencyProvider);
        }
    }

    /// <summary>
    /// Represents builder of optimal search trees.
    /// </summary>
    public class OptimalSearchTreeBuilder<TValue>
    {
        private IComparer<TValue> _comparer;
        private Func<TValue, double> _searchFrequencyProvider;

        public OptimalSearchTreeBuilder(IComparer<TValue> comparer, Func<TValue, double> searchFrequencyProvider)
        {
            if (comparer == null) throw new ArgumentNullException("comparer");
            if (searchFrequencyProvider == null) throw new ArgumentNullException("searchFrequencyProvider");
            _comparer = comparer;
            _searchFrequencyProvider = searchFrequencyProvider;
        }

        public double GetAverageSearchTime(params TValue[] searchElements)
        {
            if (searchElements == null || searchElements.Length == 0)
                throw new ArgumentNullException("searchElement");

            var orderedSearchElements = searchElements.OrderBy(e => e, _comparer).ToArray();
            var searchFrequencies = orderedSearchElements.Select(e => _searchFrequencyProvider(e)).ToArray();

            var size = orderedSearchElements.Length;

            var searchTimes = new double[size, size];

            for (int s = 0; s < size; s++)
            {
                for (int i = 0; i < size; i++)
                {
                    var j = i + s;

                    if (j >= size) continue;

                    var sumOfSearchProbabilities = GetSumOfSearchProbabilities(searchFrequencies, i, j);

                    var minSearchTime = double.MaxValue;

                    for (int r = i; r <= j; r++)
                    {
                        var searchTime = sumOfSearchProbabilities;

                        if (r - 1 >= i)
                            searchTime += searchTimes[i, r - 1];
                        if (r + 1 <= j)
                            searchTime += searchTimes[r + 1, j];

                        if (minSearchTime > searchTime)
                        { minSearchTime = searchTime; }
                    }

                    searchTimes[i, j] = minSearchTime;
                }
            }

            return searchTimes[0, size - 1];
        }

        private static double GetSumOfSearchProbabilities(double[] searchFrequencies, int i, int j)
        {
            var sumOfSearchProbabilities = 0.0;

            for (int k = i; k <= j; k++)
            {
                sumOfSearchProbabilities += searchFrequencies[k];
            }

            return sumOfSearchProbabilities;
        }
    }
}
