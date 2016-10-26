using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using EdlinSoftware.Algorithms.Visualizers;
using JetBrains.Annotations;

namespace EdlinSoftware.DataStructures.Strings
{
    /// <summary>
    /// Represents trie.
    /// </summary>
    public class Trie<TSymbol>
    {
        public readonly TrieNode<TSymbol> Root = new TrieNode<TSymbol>();

        public void Add(IEnumerable<TSymbol> pattern)
        {
            var node = Root;

            foreach (var symbol in pattern)
            {
                if (node.Edges.ContainsKey(symbol))
                {
                    var edge = node.Edges[symbol];
                    node = edge.To;
                }
                else
                {
                    var edge = new TrieEdge<TSymbol>
                    {
                        Symbol = symbol,
                        To = new TrieNode<TSymbol>()
                    };
                    node.Edges.Add(symbol, edge);
                    node = edge.To;
                }
            }

            node.IsEndOfWord = true;
        }
    }

    /// <summary>
    /// Represents node of trie.
    /// </summary>
    [DebuggerTypeProxy(typeof(TrieNodeDebuggerProxy))]
    public class TrieNode<TSymbol>
    {
        private readonly Lazy<SortedDictionary<TSymbol, TrieEdge<TSymbol>>> _edges = new Lazy<SortedDictionary<TSymbol, TrieEdge<TSymbol>>>(() => new SortedDictionary<TSymbol, TrieEdge<TSymbol>>());

        public SortedDictionary<TSymbol, TrieEdge<TSymbol>> Edges => _edges.Value;

        public bool IsEndOfWord { get; internal set; }
    }

    /// <summary>
    /// Represents edge of trie.
    /// </summary>
    public class TrieEdge<TSymbol>
    {
        public TSymbol Symbol { get; set; }
        public TrieNode<TSymbol> To { get; set; }
    }

    public class TrieNodeDebuggerProxy
    {
        private long _id = 0L;

        public readonly GraphNode Node;

        public TrieNodeDebuggerProxy([NotNull] object node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            Node = BuildNode(node);
        }

        private GraphNode BuildNode(object node)
        {
            if (node == null)
                return null;

            var edges = (IEnumerable) node.GetType().GetProperty("Edges", BindingFlags.Instance | BindingFlags.Public).GetValue(node);
            var isEndOfWord = (bool) node.GetType().GetProperty("IsEndOfWord", BindingFlags.Instance | BindingFlags.Public).GetValue(node);

            var result = new GraphNode((_id++).ToString())
            {
                Content = isEndOfWord ? "E" : ""
            };

            var children = new List<GraphEdge>();
            result.Edges = children;

            foreach (var edge in edges)
            {
                var symbol = edge.GetType().GetProperty("Symbol", BindingFlags.Instance | BindingFlags.Public).GetValue(edge);
                var to = edge.GetType().GetProperty("To", BindingFlags.Instance | BindingFlags.Public).GetValue(edge);

                children.Add(new GraphEdge
                {
                    IsDirected = true,
                    From = result,
                    Content = symbol?.ToString(),
                    To = BuildNode(to)
                });
            }

            return result;
        }
    }
}