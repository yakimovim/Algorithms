using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace EdlinSoftware.Algorithms.Strings
{
    /// <summary>
    /// Represents builder of LCP array for suffixes of text.
    /// </summary>
    /// <typeparam name="TSymbol">Type of symbols.</typeparam>
    public static class LcpArrayBuilder<TSymbol>
    {
        /// <summary>
        /// Computes LCP array for suffixes of <paramref name="text"/>.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="suffixArray">Suffix array of <paramref name="text"/>.</param>
        /// <param name="comparer">Comparer of symbols.</param>
        public static int[] ComputeLcpArray([NotNull] IReadOnlyList<TSymbol> text, int[] suffixArray, IComparer<TSymbol> comparer = null)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            comparer = comparer ?? Comparer<TSymbol>.Default;

            var lcpArray = new int[text.Count - 1];
            var lcp = 0;

            var posInOrder = InvertSuffixArray(suffixArray);
            var suffix = suffixArray[0];

            for (int i = 0; i < text.Count; i++)
            {
                var orderIndex = posInOrder[suffix];
                if (orderIndex == text.Count - 1)
                {
                    lcp = 0;
                    suffix = (suffix + 1)%text.Count;
                    continue;
                }
                var nextSuffix = suffixArray[orderIndex + 1];
                lcp = LcpOfSuffixes(text, suffix, nextSuffix, lcp - 1, comparer);
                lcpArray[orderIndex] = lcp;
                suffix = (suffix + 1) % text.Count;
            }

            return lcpArray;
        }

        private static int[] InvertSuffixArray(int[] suffixArray)
        {
            var posInOrder = new int[suffixArray.Length];

            for (int i = 0; i < posInOrder.Length; i++)
            {
                posInOrder[suffixArray[i]] = i;
            }

            return posInOrder;
        }

        private static int LcpOfSuffixes(IReadOnlyList<TSymbol> text, int i, int j, int equal, IComparer<TSymbol> comparer)
        {
            var lcp = Math.Max(0, equal);
            while (i + lcp < text.Count && j + lcp < text.Count)
            {
                if (comparer.Compare(text[i + lcp], text[j + lcp]) == 0)
                    lcp++;
                else
                    break;
            }
            return lcp;
        }
    }
}