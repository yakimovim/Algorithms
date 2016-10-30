using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace EdlinSoftware.Algorithms.Strings
{
    /// <summary>
    /// Represents implementation of Boyer-Moore-Horspool search algorithm of substring.
    /// </summary>
    /// <typeparam name="TSymbol">Type of symbols.</typeparam>
    public static class BoyerMooreHorspoolSearch<TSymbol>
    {
        /// <summary>
        /// Returns enumeration of matches of <paramref name="pattern"/> in the <paramref name="toSearch"/> string.
        /// </summary>
        /// <param name="toSearch">String to search the <paramref name="pattern"/> in.</param>
        /// <param name="pattern">Pattern to search for.</param>
        /// <param name="comparer">Comparer of symbols.</param>
        public static IEnumerable<StringSearchMatch> Search([NotNull] IReadOnlyList<TSymbol> toSearch, IReadOnlyList<TSymbol> pattern, IEqualityComparer<TSymbol> comparer = null)
        {
            if (toSearch == null) throw new ArgumentNullException(nameof(toSearch));

            if(pattern == null || pattern.Count == 0)
                yield break;

            comparer = comparer ?? EqualityComparer<TSymbol>.Default;

            var badMatchTable = ConstructBadMatchTable(pattern, comparer);

            int currentStartIndex = 0;
            while (currentStartIndex <= toSearch.Count - pattern.Count)
            {
                int charactersLeftToMatch = pattern.Count - 1;

                while (charactersLeftToMatch >= 0 &&
                    comparer.Equals(pattern[charactersLeftToMatch], toSearch[currentStartIndex + charactersLeftToMatch]))
                {
                    charactersLeftToMatch--;
                }

                if (charactersLeftToMatch < 0)
                {
                    yield return new StringSearchMatch(currentStartIndex, pattern.Count);
                    currentStartIndex += pattern.Count;
                }
                else
                {
                    currentStartIndex += badMatchTable.GetOrDefault(toSearch[currentStartIndex + pattern.Count - 1],
                        pattern.Count);
                }
            }
        }

        private static IReadOnlyDictionary<TSymbol, int> ConstructBadMatchTable(IReadOnlyList<TSymbol> pattern, IEqualityComparer<TSymbol> comparer)
        {
            var distances = new Dictionary<TSymbol, int>(comparer);

            for (int i = 0; i < pattern.Count - 1; i++)
            {
                distances[pattern[i]] = pattern.Count - i - 1;
            }

            return distances;
        }
    }
}