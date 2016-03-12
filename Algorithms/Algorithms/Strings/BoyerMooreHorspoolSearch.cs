using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace EdlinSoftware.Algorithms.Strings
{
    /// <summary>
    /// Represents implementation of Boyer-Moore-Horspool search algorithm of substring.
    /// </summary>
    public static class BoyerMooreHorspoolSearch
    {
        /// <summary>
        /// Returns enumeration of matches of <paramref name="pattern"/> in the <paramref name="toSearch"/> string.
        /// </summary>
        /// <param name="toSearch">String to search the <paramref name="pattern"/> in.</param>
        /// <param name="pattern">Pattern to search for.</param>
        public static IEnumerable<StringSearchMatch> Search([NotNull] string toSearch, string pattern, IEqualityComparer<char> comparer = null)
        {
            if (toSearch == null) throw new ArgumentNullException(nameof(toSearch));

            if(string.IsNullOrEmpty(pattern))
                yield break;

            comparer = comparer ?? EqualityComparer<char>.Default;

            var badMatchTable = ConstructBadMatchTable(pattern, comparer);

            int currentStartIndex = 0;
            while (currentStartIndex <= toSearch.Length - pattern.Length)
            {
                int charactersLeftToMatch = pattern.Length - 1;

                while (charactersLeftToMatch >= 0 &&
                    comparer.Equals(pattern[charactersLeftToMatch], toSearch[currentStartIndex + charactersLeftToMatch]))
                {
                    charactersLeftToMatch--;
                }

                if (charactersLeftToMatch < 0)
                {
                    yield return new StringSearchMatch(currentStartIndex, pattern.Length);
                    currentStartIndex += pattern.Length;
                }
                else
                {
                    currentStartIndex += badMatchTable.GetOrDefault(toSearch[currentStartIndex + pattern.Length - 1],
                        pattern.Length);
                }
            }
        }

        private static IReadOnlyDictionary<char, int> ConstructBadMatchTable(string pattern, IEqualityComparer<char> comparer)
        {
            var distances = new Dictionary<char, int>(comparer);

            for (int i = 0; i < pattern.Length - 1; i++)
            {
                distances[pattern[i]] = pattern.Length - i - 1;
            }

            return distances;
        }
    }
}