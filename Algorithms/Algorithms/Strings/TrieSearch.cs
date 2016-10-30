using System;
using System.Collections.Generic;
using EdlinSoftware.DataStructures.Strings;
using JetBrains.Annotations;

namespace EdlinSoftware.Algorithms.Strings
{
    /// <summary>
    /// Represents implementation of multiple exact pattern matching algorithm based on tries.
    /// </summary>
    /// <typeparam name="TSymbol">Type of symbols.</typeparam>
    public static class TrieSearch<TSymbol>
    {
        /// <summary>
        /// Returns enumeration of matches of words in <paramref name="patterns"/> in the <paramref name="text"/> string.
        /// </summary>
        /// <param name="text">String to search the words in <paramref name="patterns"/> in.</param>
        /// <param name="patterns">Patterns to search for.</param>
        /// <param name="comparer">Comparer of symbols.</param>
        public static IEnumerable<StringSearchMatch> Search(
            [NotNull] IReadOnlyList<TSymbol> text,
            IEnumerable<IEnumerable<TSymbol>> patterns, 
            IComparer<TSymbol> comparer = null)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            if (patterns == null)
                yield break;

            var trie = new Trie<TSymbol>(comparer);
            foreach (var pattern in patterns)
            {
                trie.Add(pattern);
            }

            for (int i = 0; i < text.Count; i++)
            {
                foreach (var match in GetMatches(text, i, trie))
                {
                    yield return match;
                }
            }
        }

        private static IEnumerable<StringSearchMatch> GetMatches(IReadOnlyList<TSymbol> text, int textIndex, Trie<TSymbol> trie)
        {
            var length = 0;

            var node = trie.Root;
            var index = textIndex;

            while (true)
            {
                if(node.IsEndOfWord)
                    yield return new StringSearchMatch(textIndex, length);

                if(index >= text.Count)
                    yield break;

                if (node.Edges.ContainsKey(text[index]))
                {
                    var edge = node.Edges[text[index]];
                    node = edge.To;
                    index++;
                    length++;
                }
                else
                {
                    yield break;
                }
            }
        }
    }
}