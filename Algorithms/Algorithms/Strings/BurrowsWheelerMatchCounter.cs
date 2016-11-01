using System;
using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.Algorithms.Collections.Sorting;
using EdlinSoftware.DataStructures.Strings;

namespace EdlinSoftware.Algorithms.Strings
{
    /// <summary>
    /// Represents counter of matches of pattern in the text represented by Burrows-Wheeler transformation.
    /// </summary>
    /// <typeparam name="TSymbol">Type of text symbols.</typeparam>
    public static class BurrowsWheelerMatchCounter<TSymbol>
    {
        /// <summary>
        /// Returns number of matches in the text presented by Burrows-Wheeler <see cref="transformation"/> for each pattern in <see cref="patterns"/>.
        /// </summary>
        /// <param name="transformation">Burrows-Wheeler transformation of text.</param>
        /// <param name="patterns">Patterns.</param>
        /// <param name="stopSymbol">Stop symbol.</param>
        /// <param name="comparer">Comparer of symbols.</param>
        /// <returns></returns>
        public static IEnumerable<int> GetNumberOfMatches(
            IReadOnlyList<TSymbol> transformation,
            IEnumerable<IReadOnlyList<TSymbol>> patterns, 
            TSymbol stopSymbol, 
            IComparer<TSymbol> comparer = null)
        {
            var firstOccurencesOfSymbolsInTheFirstColumn = GetFirstOccurencesOfSymbolsInTheFirstColumn(transformation, comparer, stopSymbol);

            var symbolCounts = GetSymbolCounts(transformation, firstOccurencesOfSymbolsInTheFirstColumn.Keys.ToArray());

            foreach (var pattern in patterns)
            {
                yield return GetMatchesCount(transformation, pattern, firstOccurencesOfSymbolsInTheFirstColumn, symbolCounts);
            }
        }

        private static IReadOnlyDictionary<TSymbol, int[]> GetSymbolCounts(IReadOnlyList<TSymbol> transformation, TSymbol[] differentSymbols)
        {
            var symbolCounts = differentSymbols.ToDictionary(s => s, s => new int[transformation.Count + 1]);

            for (int i = 0; i < transformation.Count; i++)
            {
                foreach (var symbol in differentSymbols)
                {
                    symbolCounts[symbol][i + 1] = symbolCounts[symbol][i];
                }

                symbolCounts[transformation[i]][i + 1]++;
            }


            return symbolCounts;
        }

        private static IReadOnlyDictionary<TSymbol, int> GetFirstOccurencesOfSymbolsInTheFirstColumn(IReadOnlyList<TSymbol> transformation, IComparer<TSymbol> comparer, TSymbol stopSymbol)
        {
            var countOfSymbols = new Dictionary<TSymbol, int>();
            foreach (var symbol in transformation)
            {
                if (countOfSymbols.ContainsKey(symbol))
                    countOfSymbols[symbol]++;
                else
                    countOfSymbols[symbol] = 1;
            }

            var differentSymbols = countOfSymbols.Keys.ToArray();

            var rnd = new Random((int)DateTime.UtcNow.Ticks);

            var pivotSelector = new Func<IList<TSymbol>, int, int, int>((list, left, right) => 1 + rnd.Next(left - 1, right));

            var sorter = new QuickSorter<TSymbol>(pivotSelector, new StopSymbolFirstComparer<TSymbol>(comparer ?? Comparer<TSymbol>.Default, stopSymbol));

            sorter.Sort(differentSymbols);

            var firstOccurencesOfSymbolsInTheFirstColumn = new Dictionary<TSymbol, int>();
            var currentPosition = 0;

            foreach (var symbol in differentSymbols)
            {
                firstOccurencesOfSymbolsInTheFirstColumn[symbol] = currentPosition;
                currentPosition += countOfSymbols[symbol];
            }

            return firstOccurencesOfSymbolsInTheFirstColumn;
        }

        private static int GetMatchesCount(IReadOnlyList<TSymbol> transformation, IReadOnlyList<TSymbol> pattern, IReadOnlyDictionary<TSymbol, int> firstOccurencesOfSymbolsInTheFirstColumn, IReadOnlyDictionary<TSymbol, int[]> symbolCounts)
        {
            var top = 0;
            var bottom = transformation.Count - 1;

            var patternIndex = pattern.Count - 1;
            while (top <= bottom)
            {
                if (patternIndex < 0)
                    return bottom - top + 1;

                var symbol = pattern[patternIndex--];
                if (!symbolCounts.ContainsKey(symbol))
                    return 0;

                if (symbolCounts[symbol][top] == symbolCounts[symbol][bottom + 1])
                    return 0;

                top = firstOccurencesOfSymbolsInTheFirstColumn[symbol] + symbolCounts[symbol][top];
                bottom = firstOccurencesOfSymbolsInTheFirstColumn[symbol] + symbolCounts[symbol][bottom + 1] - 1;
            }

            return 0;
        }
    }
}