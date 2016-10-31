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

    }
}