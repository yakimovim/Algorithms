using System;
using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.DataStructures.Strings;
using JetBrains.Annotations;

namespace EdlinSoftware.Algorithms.Strings
{
    /// <summary>
    /// Represents class for multiple exact pattern matching using suffix array.
    /// </summary>
    /// <typeparam name="TSymbol">Type of symbols.</typeparam>
    public static class SuffixArraySearch<TSymbol>
    {
        /// <summary>
        /// Returns enumeration of matches of words in <paramref name="patterns"/> in the <paramref name="text"/> string.
        /// </summary>
        /// <param name="text">String to search the words in <paramref name="patterns"/> in.</param>
        /// <param name="stopSymbol">Stop symbol different from all symbols of <paramref name="text"/>.</param>
        /// <param name="patterns">Patterns to search for.</param>
        /// <param name="comparer">Comparer of symbols.</param>
        public static IEnumerable<StringSearchMatch> Search(
            [NotNull] IReadOnlyList<TSymbol> text,
            TSymbol stopSymbol,
            IEnumerable<IEnumerable<TSymbol>> patterns,
            IComparer<TSymbol> comparer = null)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            if (patterns == null)
                yield break;

            comparer = new StopSymbolFirstComparer<TSymbol>(comparer ?? Comparer<TSymbol>.Default, stopSymbol);

            text = GetText(text, stopSymbol);

            var suffixArray = SuffixArrayCreator<TSymbol>.GetSuffixArrayFast(text, comparer);

            foreach (var pattern in patterns)
            {
                foreach (var match in GetMatches(text, pattern.ToArray(), suffixArray, comparer))
                {
                    yield return match;
                }
            }
        }

        private static IEnumerable<StringSearchMatch> GetMatches(IReadOnlyList<TSymbol> text, TSymbol[] pattern, int[] suffixArray, IComparer<TSymbol> comparer)
        {
            var start = GetLowerBound(text, pattern, suffixArray, comparer);

            var end = GetUpperBound(text, pattern, suffixArray, comparer, start);

            if (start >=0 && start < text.Count && Compare(pattern, text, suffixArray[start], comparer) != 0)
                start++;

            if (end >= 0 && end < text.Count && Compare(pattern, text, suffixArray[end], comparer) != 0)
                end--;

            for (int i = start; i <= end; i++)
            {
                yield return new StringSearchMatch(suffixArray[i], pattern.Length);
            }
        }

        private static int GetLowerBound(IReadOnlyList<TSymbol> text, TSymbol[] pattern, int[] suffixArray, IComparer<TSymbol> comparer)
        {
            var minIndex = 0;
            var maxIndex = text.Count;
            while (minIndex < maxIndex)
            {
                var midIndex = (minIndex + maxIndex) / 2;
                var compare = Compare(pattern, text, suffixArray[midIndex], comparer);
                if (compare > 0)
                    minIndex = midIndex + 1;
                else if (compare == 0)
                    maxIndex = midIndex;
                else
                    maxIndex = midIndex - 1;
            }
            return minIndex;
        }

        private static int GetUpperBound(IReadOnlyList<TSymbol> text, TSymbol[] pattern, int[] suffixArray,
            IComparer<TSymbol> comparer, int lowerBound)
        {
            var minIndex = lowerBound;
            var maxIndex = text.Count;
            while (minIndex < maxIndex)
            {
                var midIndex = (minIndex + maxIndex) / 2;
                var compare = Compare(pattern, text, suffixArray[midIndex], comparer);
                if (compare < 0)
                    maxIndex = midIndex - 1;
                else if (compare == 0)
                {
                    if(minIndex != midIndex)
                        minIndex = midIndex;
                    else
                        break;
                }
                else
                    minIndex = midIndex + 1;
            }

            if (maxIndex == text.Count)
                maxIndex--;

            return maxIndex;
        }

        private static int Compare(TSymbol[] pattern, IReadOnlyList<TSymbol> text, int textPos, IComparer<TSymbol> comparer)
        {
            foreach (var patternSymbol in pattern)
            {
                var comparison = comparer.Compare(patternSymbol, text[textPos]);
                if (comparison != 0)
                    return comparison;
                textPos++;
            }

            return 0;
        }

        private static IReadOnlyList<TSymbol> GetText(IReadOnlyList<TSymbol> text, TSymbol stopSymbol)
        {
            if (text.Count == 0)
                return new List<TSymbol> { stopSymbol };

            return text[text.Count - 1].Equals(stopSymbol) ? text : new List<TSymbol>(text) { stopSymbol };
        }
    }
}