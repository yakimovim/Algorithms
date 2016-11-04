using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace EdlinSoftware.Algorithms.Strings
{
    /// <summary>
    /// Represents prefix function used in Knuth-Morris-Pratt algorithm.
    /// </summary>
    /// <typeparam name="TSymbol">Types of symbols.</typeparam>
    public static class PrefixFunction<TSymbol>
    {
        /// <summary>
        /// Returns prefix function for given <paramref name="text"/>.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="comparer">Comparer of symbols.</param>
        public static IReadOnlyList<int> GetPrefixFunction([NotNull] IReadOnlyList<TSymbol> text, IEqualityComparer<TSymbol> comparer = null)
        {
            comparer = comparer ?? EqualityComparer<TSymbol>.Default;

            if (text == null) throw new ArgumentNullException(nameof(text));

            var prefixFunction = new List<int>();
            if (text.Count == 0)
                return prefixFunction;

            prefixFunction.Add(0);
            var border = 0;
            for (int i = 1; i < text.Count; i++)
            {
                while ((border > 0) && !comparer.Equals(text[i], text[border]))
                    border = prefixFunction[border - 1];

                if (comparer.Equals(text[i], text[border]))
                    border = border + 1;
                else
                    border = 0;
                prefixFunction.Add(border);
            }

            return prefixFunction;
        }
    }
}