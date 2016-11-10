using System;
using System.Collections.Generic;
using EdlinSoftware.DataStructures.Strings;
using JetBrains.Annotations;

namespace EdlinSoftware.Algorithms.Strings
{
    /// <summary>
    /// Represents naive creator of suffix tree.
    /// </summary>
    /// <typeparam name="TSymbol">Type of symbols.</typeparam>
    public static class NaiveSuffixTreeCreator<TSymbol>
    {
        public static SuffixTree<TSymbol> CreateSuffixTree(
            [NotNull] IReadOnlyList<TSymbol> text,
            TSymbol stopSymbol,
            [CanBeNull] IComparer<TSymbol> comparer = null)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            text = text.FinishWith(stopSymbol);

            comparer = new StopSymbolFirstComparer<TSymbol>(comparer ?? Comparer<TSymbol>.Default, stopSymbol);

            var root = new SuffixTreeNode<TSymbol>(comparer);

            for (uint i = 0; i < text.Count; i++)
            {
                AddSuffix(root, text, comparer, i);
            }

            return new SuffixTree<TSymbol>(text, root);
        }

        private static void AddSuffix(SuffixTreeNode<TSymbol> root, IReadOnlyList<TSymbol> text, IComparer<TSymbol> comparer, uint startIndex)
        {
            var node = root;
            var index = startIndex;
            var restLength = (uint)text.Count - index;

            while (true)
            {
                var firstSymbol = text[(int)index];

                var edge = FindEdge(node, firstSymbol);

                if (edge == null)
                {
                    edge = new SuffixTreeEdge<TSymbol>
                    {
                        Start = index,
                        Length = restLength,
                        From = node,
                        To = new SuffixTreeNode<TSymbol>(comparer)
                        {
                            StringDepth = node.StringDepth + restLength,
                            SuffixStart = startIndex
                        }
                    };
                    node.Edges.Add(firstSymbol, edge);
                    return;
                }
                else
                {
                    var matchLength = GetMatchLength(text, edge, index);
                    if (matchLength == edge.Length)
                    {
                        node = edge.To;
                        index += matchLength;
                        restLength -= matchLength;
                    }
                    else
                    {
                        var edge1 = new SuffixTreeEdge<TSymbol>
                        {
                            Start = edge.Start,
                            Length = matchLength,
                            From = node,
                            To = new SuffixTreeNode<TSymbol>(comparer)
                            {
                                StringDepth = node.StringDepth + matchLength
                            }
                        };

                        var edge2 = new SuffixTreeEdge<TSymbol>
                        {
                            Start = edge.Start + matchLength,
                            Length = edge.Length - matchLength,
                            From = edge1.To,
                            To = edge.To
                        };

                        edge1.To.Edges.Add(text[(int)edge2.Start], edge2);
                        node.Edges[firstSymbol] = edge1;

                        node = edge1.To;
                        index += matchLength;
                        restLength -= matchLength;
                    }
                }
            }
        }

        private static SuffixTreeEdge<TSymbol> FindEdge(SuffixTreeNode<TSymbol> node, TSymbol symbol)
        {
            if (node.Edges.ContainsKey(symbol))
            {
                return node.Edges[symbol];
            }

            return null;
        }

        private static uint GetMatchLength(IReadOnlyList<TSymbol> text, SuffixTreeEdge<TSymbol> edge, uint index)
        {
            var edgeIndex = (int)edge.Start;
            var suffixIndex = (int)index;

            var matchLength = 0U;

            while (true)
            {
                if (!text[edgeIndex].Equals(text[suffixIndex]))
                    break;
                matchLength++;
                edgeIndex++;
                suffixIndex++;
                if (edgeIndex >= edge.Start + edge.Length)
                    break;
                if (suffixIndex >= text.Count)
                    break;
            }

            return matchLength;
        }
    }
}