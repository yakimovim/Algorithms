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

        public static int[] GetSuffixArrayFast(IReadOnlyList<TSymbol> text, IComparer<TSymbol> comparer = null)
        {
            comparer = comparer ?? Comparer<TSymbol>.Default;

            var order = SortCharacters(text, comparer);
            var equivalenceClasses = ComputeCharEquivalenceClasses(text, comparer, order);

            var partialCyclicRotationLength = 1;
            while (partialCyclicRotationLength < text.Count)
            {
                order = SortDoubled(text, partialCyclicRotationLength, order, equivalenceClasses);
                equivalenceClasses = UpdateEquivalenceClasses(order, equivalenceClasses, partialCyclicRotationLength);
                partialCyclicRotationLength *= 2;
            }

            return order;
        }

        private static int[] SortCharacters(IReadOnlyList<TSymbol> text, IComparer<TSymbol> comparer)
        {
            var order = new int[text.Count];

            var counts = new SortedDictionary<TSymbol, int>(comparer);

            foreach (var value in text)
            {
                if (!counts.ContainsKey(value))
                    counts[value] = 1;
                else
                    counts[value]++;
            }

            var sortedValues = counts.Keys.ToArray();

            for (int i = 1; i < sortedValues.Length; i++)
            {
                counts[sortedValues[i]] += counts[sortedValues[i - 1]];
            }

            for (int i = text.Count - 1; i >= 0; i--)
            {
                var symbol = text[i];

                counts[symbol]--;

                order[counts[symbol]] = i;
            }

            return order;
        }

        private static int[] ComputeCharEquivalenceClasses(IReadOnlyList<TSymbol> text, IComparer<TSymbol> comparer, int[] order)
        {
            var classes = new int[text.Count];

            classes[order[0]] = 0;

            for (int i = 1; i < order.Length; i++)
            {
                if (comparer.Compare(text[order[i]], text[order[i - 1]]) != 0)
                {
                    classes[order[i]] = classes[order[i - 1]] + 1;
                }
                else
                {
                    classes[order[i]] = classes[order[i - 1]];
                }
            }

            return classes;
        }

        private static int[] SortDoubled(IReadOnlyList<TSymbol> text, int partialCyclicRotationLength, int[] order, int[] equivalenceClasses)
        {
            var newOrder = new int[text.Count];

            var counts = new int[text.Count];

            for (int i = 0; i < text.Count; i++)
            {
                counts[equivalenceClasses[i]]++;
            }

            for (int i = 1; i < text.Count; i++)
            {
                counts[i] += counts[i - 1];
            }

            for (int i = text.Count - 1; i >= 0; i--)
            {
                var start = (order[i] - partialCyclicRotationLength + text.Count) % text.Count;
                var equivalenceClass = equivalenceClasses[start];
                counts[equivalenceClass]--;
                newOrder[counts[equivalenceClass]] = start;
            }

            return newOrder;
        }

        private static int[] UpdateEquivalenceClasses(int[] newOrder, int[] equivalenceClasses, int partialCyclicRotationLength)
        {
            var newClasses = new int[newOrder.Length];
            newClasses[newOrder[0]] = 0;

            for (int i = 1; i < newOrder.Length; i++)
            {
                var current = newOrder[i];
                var prev = newOrder[i - 1];
                var middle = (current + partialCyclicRotationLength) % newOrder.Length;
                var prevMiddle = (prev + partialCyclicRotationLength) % newOrder.Length;

                if (equivalenceClasses[current] != equivalenceClasses[prev]
                    || equivalenceClasses[middle] != equivalenceClasses[prevMiddle])
                {
                    newClasses[current] = newClasses[prev] + 1;
                }
                else
                {
                    newClasses[current] = newClasses[prev];
                }
            }

            return newClasses;
        }
    }
}