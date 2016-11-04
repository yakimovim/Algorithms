using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace EdlinSoftware.Algorithms.Strings
{
    /// <summary>
    /// Represents implementation of Knuth-Morris-Pratt search algorithm of substring.
    /// </summary>
    /// <typeparam name="TSymbol">Type of symbols.</typeparam>
    public static class KnuthMorrisPrattSearch<TSymbol>
    {
        /// <summary>
        /// Returns enumeration of matches of <paramref name="pattern"/> in the <paramref name="toSearch"/> string.
        /// </summary>
        /// <param name="toSearch">String to search the <paramref name="pattern"/> in.</param>
        /// <param name="pattern">Pattern to search for.</param>
        /// <param name="stopSymbol">Stop symbol.</param>
        /// <param name="comparer">Comparer of symbols.</param>
        public static IEnumerable<StringSearchMatch> Search([NotNull] IReadOnlyList<TSymbol> toSearch,
            IReadOnlyList<TSymbol> pattern, TSymbol stopSymbol, IEqualityComparer<TSymbol> comparer = null)
        {
            if (toSearch == null) throw new ArgumentNullException(nameof(toSearch));

            if (pattern == null || pattern.Count == 0)
                yield break;

            var combinedText = new List<TSymbol>(pattern) {stopSymbol};
            combinedText.AddRange(toSearch);

            var prefixFunction = PrefixFunction<TSymbol>.GetPrefixFunction(combinedText, comparer);

            for (int i = pattern.Count + 1; i < combinedText.Count; i++)
            {
                if(prefixFunction[i] == pattern.Count)
                    yield return new StringSearchMatch(i - 2 * pattern.Count, pattern.Count);
            }
        }
    }
}