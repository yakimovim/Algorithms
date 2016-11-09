using System;
using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.DataStructures.Strings;
using JetBrains.Annotations;

namespace EdlinSoftware.Algorithms.Strings
{
    public static class SuffixTreeSearch<TSymbol>
    {
        /// <summary>
        /// Returns enumeration of matches of words in <paramref name="patterns"/> in the <paramref name="text"/> string.
        /// </summary>
        /// <param name="text">String to search the words in <paramref name="patterns"/> in.</param>
        /// <param name="stopSymbol">Stop symbol different from all symbols of <paramref name="text"/>.</param>
        /// <param name="patterns">Patterns to search for.</param>
        /// <param name="comparer">Comparer of symbols.</param>
        public static IEnumerable<StringSearchMatch> Search(
            [NotNull] IReadOnlyList<TSymbol> text,
            TSymbol stopSymbol,
            IEnumerable<IEnumerable<TSymbol>> patterns,
            IComparer<TSymbol> comparer = null)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            if (patterns == null)
                yield break;

            var suffixTree = new SuffixTree<TSymbol>(text, stopSymbol, comparer);

            foreach (var pattern in patterns)
            {
                foreach (var match in GetMatches(pattern.ToArray(), suffixTree))
                {
                    yield return match;
                }
            }
        }

        private static IEnumerable<StringSearchMatch> GetMatches(TSymbol[] pattern, SuffixTree<TSymbol> suffixTree)
        {
            SuffixTreeNode<TSymbol> node = GetPatternNode(pattern, suffixTree);
            if(node == null)
                yield break; 

            var starts = GetSuffixStarts(node);

            foreach (var start in starts.Where(s => s < suffixTree.Text.Count - 1))
            {
                yield return new StringSearchMatch(start, pattern.Length);
            }
        }

        private static SuffixTreeNode<TSymbol> GetPatternNode(TSymbol[] pattern, SuffixTree<TSymbol> suffixTree)
        {
            if (pattern.Length == 0)
                return suffixTree.Root;

            var lastEdge = GetLastEdge(pattern, suffixTree);

            return lastEdge?.To;
        }

        private static SuffixTreeEdge<TSymbol> GetLastEdge(TSymbol[] pattern, SuffixTree<TSymbol> suffixTree)
        {
            var node = suffixTree.Root;
            var patternIndex = 0;

            while (true)
            {
                if (!node.Edges.ContainsKey(pattern[patternIndex]))
                    return null;

                var edge = node.Edges[pattern[patternIndex]];

                var matchLength = GetMatchLength(edge, suffixTree.Text, pattern, patternIndex);

                patternIndex += matchLength;

                if (patternIndex >= pattern.Length)
                    return edge;

                if (matchLength < edge.Length)
                    return null;

                node = edge.To;
            }

        }

        private static int GetMatchLength(SuffixTreeEdge<TSymbol> edge, IReadOnlyList<TSymbol> edgeText, TSymbol[] pattern, int patternIndex)
        {
            var edgeIndex = (int)edge.Start;

            var matchLength = 0;

            while (true)
            {
                if (!edgeText[edgeIndex].Equals(pattern[patternIndex]))
                    break;
                matchLength++;
                edgeIndex++;
                patternIndex++;
                if (edgeIndex >= edge.Start + edge.Length)
                    break;
                if (patternIndex >= pattern.Length)
                    break;
            }

            return matchLength;
        }

        private static IEnumerable<int> GetSuffixStarts(SuffixTreeNode<TSymbol> node)
        {
            var queue = new Queue<SuffixTreeNode<TSymbol>>();
            queue.Enqueue(node);

            while (queue.Count > 0)
            {
                node = queue.Dequeue();
                if (node.SuffixStart.HasValue)
                    yield return (int) node.SuffixStart.Value;

                foreach (var edge in node.Edges.Values)
                {
                    queue.Enqueue(edge.To);
                }
            }
        }
    }
}