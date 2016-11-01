using System;
using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.Algorithms.Collections.Sorting;
using EdlinSoftware.DataStructures.Strings;
using JetBrains.Annotations;

namespace EdlinSoftware.Algorithms.Strings
{
    /// <summary>
    /// Represents Burrows--Wheeler transformer of strings.
    /// </summary>
    /// <typeparam name="TSymbol">Type of string symbols.</typeparam>
    public static class BurrowsWheelerTransformer<TSymbol>
    {
        public static IReadOnlyList<TSymbol> Transform(
            [NotNull] IReadOnlyList<TSymbol> text, 
            TSymbol stopSymbol,
            IComparer<TSymbol> comparer = null)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            text = GetText(text, stopSymbol);

            var rotations =
                Enumerable.Range(1, text.Count).Select(i => new StringCyclicRotation<TSymbol>(text, -i)).ToArray();

            var rnd = new Random((int)DateTime.UtcNow.Ticks);

            var pivotSelector = new Func<IList<StringCyclicRotation<TSymbol>>, int, int, int>((list, left, right) => 1 + rnd.Next(left - 1, right));

            var sorter = new QuickSorter<StringCyclicRotation<TSymbol>>(pivotSelector, new StringCyclicRotationComparer<TSymbol>(new StopSymbolFirstComparer<TSymbol>(comparer ?? Comparer<TSymbol>.Default, stopSymbol)));

            sorter.Sort(rotations);

            return rotations.Select(r => r[text.Count - 1]).ToList();
        }

        private static IReadOnlyList<TSymbol> GetText(IReadOnlyList<TSymbol> text, TSymbol stopSymbol)
        {
            if (text.Count == 0)
                return new List<TSymbol> { stopSymbol };

            return text[text.Count - 1].Equals(stopSymbol) ? text : new List<TSymbol>(text) { stopSymbol };
        }


        public static IReadOnlyList<TSymbol> InverseTransform(
            [NotNull] IReadOnlyList<TSymbol> transformation,
            TSymbol stopSymbol,
            IComparer<TSymbol> comparer = null)
        {
            var lastColumn = transformation;
            var firstColumn = GetFirstColumn(transformation, stopSymbol, comparer);

            var lastToFirstMap = GetLastToFirstMap(lastColumn, firstColumn);

            return GetInverseTransform(firstColumn, lastColumn, lastToFirstMap);
        }

        private static IReadOnlyList<TSymbol> GetFirstColumn(IReadOnlyList<TSymbol> transformation, TSymbol stopSymbol, IComparer<TSymbol> comparer)
        {
            var firstColumn = transformation.ToList();

            var rnd = new Random((int)DateTime.UtcNow.Ticks);

            var pivotSelector = new Func<IList<TSymbol>, int, int, int>((list, left, right) => 1 + rnd.Next(left - 1, right));

            var sorter = new QuickSorter<TSymbol>(pivotSelector, new StopSymbolFirstComparer<TSymbol>(comparer ?? Comparer<TSymbol>.Default, stopSymbol));

            sorter.Sort(firstColumn);

            return firstColumn;
        }

        private static int[] GetLastToFirstMap(IReadOnlyList<TSymbol> lastColumn, IReadOnlyList<TSymbol> firstColumn)
        {
            var firstOccurencesOfSymbolsInTheFirstColumn = GetFirstOccurencesOfSymbolsInTheFirstColumn(firstColumn);

            var lastToFirstMap = new int[lastColumn.Count];

            var symbolsCount = new Dictionary<TSymbol, int>();

            for (int i = 0; i < lastColumn.Count; i++)
            {
                var symbol = lastColumn[i];

                int symbolCount;
                if (!symbolsCount.TryGetValue(symbol, out symbolCount))
                {
                    symbolCount = 0;
                }
                symbolCount++;
                symbolsCount[symbol] = symbolCount;

                lastToFirstMap[i] = firstOccurencesOfSymbolsInTheFirstColumn[symbol] + symbolCount - 1;
            }

            return lastToFirstMap;
        }

        private static IReadOnlyDictionary<TSymbol, int> GetFirstOccurencesOfSymbolsInTheFirstColumn(IReadOnlyList<TSymbol> firstColumn)
        {
            var firstOccurencesOfSymbolsInTheFirstColumn = new Dictionary<TSymbol, int>();

            for (int i = 0; i < firstColumn.Count; i++)
            {
                var symbol = firstColumn[i];

                if(firstOccurencesOfSymbolsInTheFirstColumn.ContainsKey(symbol))
                    continue;

                firstOccurencesOfSymbolsInTheFirstColumn[symbol] = i;
            }

            return firstOccurencesOfSymbolsInTheFirstColumn;
        }

        private static IReadOnlyList<TSymbol> GetInverseTransform(IReadOnlyList<TSymbol> firstColumn, IReadOnlyList<TSymbol> lastColumn, int[] lastToFirstMap)
        {
            var originalText = new TSymbol[firstColumn.Count];
            originalText[originalText.Length - 1] = firstColumn[0];
            var currentRow = 0;

            for (int i = originalText.Length - 2; i >= 0; i--)
            {
                originalText[i] = lastColumn[currentRow];
                currentRow = lastToFirstMap[currentRow];
            }

            return originalText;
        }
    }
}