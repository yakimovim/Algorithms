using System;
using System.Collections.Generic;
using EdlinSoftware.DataStructures.Strings;
using JetBrains.Annotations;

namespace EdlinSoftware.Algorithms.Strings
{
    /// <summary>
    /// Represents creator of suffix tree using suffix array.
    /// </summary>
    /// <typeparam name="TSymbol">Type of symbols.</typeparam>
    public class SuffixTreeCreator<TSymbol>
    {
        /// <summary>
        /// Creates suffix tree for <paramref name="text"/>.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="stopSymbol">Stop symbol.</param>
        /// <param name="comparer">Comparer of symbols.</param>
        public static SuffixTree<TSymbol> CreateSuffixTree(
            [NotNull] IReadOnlyList<TSymbol> text, 
            TSymbol stopSymbol,
            [CanBeNull] IComparer<TSymbol> comparer = null)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            text = GetText(text, stopSymbol);

            comparer = new StopSymbolFirstComparer<TSymbol>(comparer ?? Comparer<TSymbol>.Default, stopSymbol);

            var suffixArray = SuffixArrayCreator<TSymbol>.GetSuffixArrayFast(text, comparer);

            var lcpArray = LcpArrayCreator<TSymbol>.ComputeLcpArray(text, suffixArray, comparer);

            return CreateSuffixTree(text, suffixArray, lcpArray, comparer);
        }

        private static IReadOnlyList<TSymbol> GetText(IReadOnlyList<TSymbol> text, TSymbol stopSymbol)
        {
            if (text.Count == 0)
                return new List<TSymbol> { stopSymbol };

            return text[text.Count - 1].Equals(stopSymbol) ? text : new List<TSymbol>(text) { stopSymbol };
        }

        private static SuffixTree<TSymbol> CreateSuffixTree(IReadOnlyList<TSymbol> text, int[] suffixArray, int[] lcpArray, IComparer<TSymbol> comparer)
        {
            var root = new SuffixTreeNode<TSymbol>(comparer);

            var lcpPrev = 0;
            var currentNode = root;

            for (int i = 0; i < text.Count; i++)
            {
                var positionOfCurrentSuffixStart = suffixArray[i];

                while (currentNode.StringDepth > lcpPrev)
                { currentNode = currentNode.InEdge.From; }

                if (currentNode.StringDepth == lcpPrev)
                {
                    currentNode = CreateNewLeaf(currentNode, text, positionOfCurrentSuffixStart, comparer);
                }
                else // currentNode.StringDepth < lcpPrev
                {
                    var offset = lcpPrev - currentNode.StringDepth;
                    var edgeStart = suffixArray[i - 1] + currentNode.StringDepth;
                    var midNode = BreakEdge(text, currentNode, edgeStart, offset, comparer);
                    currentNode = CreateNewLeaf(midNode, text, positionOfCurrentSuffixStart, comparer);
                }

                if (i < text.Count - 1)
                    lcpPrev = lcpArray[i];
            }

            return new SuffixTree<TSymbol>(text, root);
        }

        private static SuffixTreeNode<TSymbol> CreateNewLeaf(SuffixTreeNode<TSymbol> node, IReadOnlyList<TSymbol> text, int positionOfCurrentSuffixStart, IComparer<TSymbol> comparer)
        {
            var leaf = new SuffixTreeNode<TSymbol>(comparer)
            {
                StringDepth = (uint) (text.Count - positionOfCurrentSuffixStart),
                SuffixStart = (uint) positionOfCurrentSuffixStart
            };
            var edge = new SuffixTreeEdge<TSymbol>
            {
                From = node,
                To = leaf,
                Start = node.StringDepth + (uint)positionOfCurrentSuffixStart
            };
            edge.Length = (uint)text.Count - edge.Start;
            node.Edges[text[(int)edge.Start]] = edge;
            return leaf;
        }

        private static SuffixTreeNode<TSymbol> BreakEdge(IReadOnlyList<TSymbol> text, SuffixTreeNode<TSymbol> node, long start, long offset, IComparer<TSymbol> comparer)
        {
            var startChar = text[(int)start];
            var midChar = text[(int)(start + offset)];
            var midNode = new SuffixTreeNode<TSymbol>(comparer)
            {
                StringDepth = (uint) (node.StringDepth + offset)
            };

            var edgeToBreak = node.Edges[startChar];

            var edge1 = new SuffixTreeEdge<TSymbol>
            {
                From = node,
                To = midNode,
                Start = edgeToBreak.Start,
                Length = (uint) offset
            };
            var edge2 = new SuffixTreeEdge<TSymbol>
            {
                From = midNode,
                To = edgeToBreak.To,
                Start = (uint) (edgeToBreak.Start + offset),
                Length = (uint) (edgeToBreak.Length - offset)
            };

            node.Edges[startChar] = edge1;
            midNode.Edges[midChar] = edge2;

            return midNode;
        }
    }
}