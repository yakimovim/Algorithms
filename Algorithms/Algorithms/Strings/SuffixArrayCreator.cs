using System;
using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.DataStructures.Strings;
using JetBrains.Annotations;

namespace EdlinSoftware.Algorithms.Strings
{
    /// <summary>
    /// Represents creator of suffix array.
    /// </summary>
    /// <typeparam name="TSymbol">Type of symbols.</typeparam>
    public static class SuffixArrayCreator<TSymbol>
    {
        private class StringCyclicRotationWithStart : StringCyclicRotation<TSymbol>
        {
            public int Start { get; }

            public StringCyclicRotationWithStart([NotNull] IReadOnlyList<TSymbol> text, int offset) 
                : base(text, offset)
            {
                Start = Offset;
                while (Start >= text.Count)
                {
                    Start -= text.Count;
                }
            }
        }

        private class StringCyclicRotationWithStartComparer : IComparer<StringCyclicRotationWithStart>
        {
            private readonly StringCyclicRotationComparer<TSymbol> _comparer;

            public StringCyclicRotationWithStartComparer(IComparer<TSymbol> comparer = null)
            {
                _comparer = new StringCyclicRotationComparer<TSymbol>(comparer);
            }

            public int Compare(StringCyclicRotationWithStart x, StringCyclicRotationWithStart y)
            {
                return _comparer.Compare(x, y);
            }
        }

        public static int[] GetSuffixArray(IReadOnlyList<TSymbol> text, IComparer<TSymbol> comparer = null)
        {
            var rotations = Enumerable.Range(1, text.Count).Select(i => new StringCyclicRotationWithStart(text, -i)).ToArray();

            Array.Sort(rotations, new StringCyclicRotationWithStartComparer(comparer ?? Comparer<TSymbol>.Default));

            return rotations.Select(r => r.Start).ToArray();
        }
    }
}