using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using EdlinSoftware.Algorithms.Visualizers;
using JetBrains.Annotations;
using System.Linq;

namespace EdlinSoftware.DataStructures.Strings
{
    /// <summary>
    /// Represents suffix tree for one string.
    /// </summary>
    /// <typeparam name="TSymbol">Type of string symbol.</typeparam>
    [DebuggerTypeProxy(typeof(SuffixTreeDebuggerProxy))]
    public class SuffixTree<TSymbol>
    {
        private readonly IReadOnlyList<TSymbol> _text;

        public readonly SuffixTreeNode<TSymbol> Root = new SuffixTreeNode<TSymbol>();

        public SuffixTree(IReadOnlyList<TSymbol> text)
        {
            _text = text;

            for (uint i = 0; i < text.Count; i++)
            {
                AddSuffix(text, i);
            }
        }

        private void AddSuffix(IReadOnlyList<TSymbol> text, uint startIndex)
        {
            var node = Root;
            var index = startIndex;
            var restLength = (uint)text.Count - index;

            while (true)
            {
                var firstSymbol = text[(int)index];

                var edge = FindEdge(node, firstSymbol);

                if (edge == null)
                {
                    edge = new SuffixTreeEdge<TSymbol>();
                    edge.Start = index;
                    edge.Length = restLength;
                    edge.To = new SuffixTreeNode<TSymbol>();
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
                        var edge1 = new SuffixTreeEdge<TSymbol>();
                        edge1.Start = edge.Start;
                        edge1.Length = matchLength;
                        edge1.To = new SuffixTreeNode<TSymbol>();

                        var edge2 = new SuffixTreeEdge<TSymbol>();
                        edge2.Start = edge.Start + matchLength;
                        edge2.Length = edge.Length - matchLength;
                        edge2.To = edge.To;

                        edge1.To.Edges.Add(text[(int)edge2.Start], edge2);
                        node.Edges[firstSymbol] = edge1;

                        node = edge1.To;
                        index += matchLength;
                        restLength -= matchLength;
                    }
                }
            }
        }

        private SuffixTreeEdge<TSymbol> FindEdge(SuffixTreeNode<TSymbol> node, TSymbol symbol)
        {
            if (node.Edges.ContainsKey(symbol))
            {
                return node.Edges[symbol];
            }

            return null;
        }

        private uint GetMatchLength(IReadOnlyList<TSymbol> text, SuffixTreeEdge<TSymbol> edge, uint index)
        {
            var edgeIndex = (int)edge.Start;
            var suffixIndex = (int)index;

            var matchLength = 0U;

            while (true)
            {
                if (text[edgeIndex].Equals(text[suffixIndex]))
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

    /// <summary>
    /// Represents node of suffix tree.
    /// </summary>
    /// <typeparam name="TSymbol">Type of string symbol.</typeparam>
    [DebuggerTypeProxy(typeof(SuffixTreeNodeDebuggerProxy))]
    public class SuffixTreeNode<TSymbol>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<SortedDictionary<TSymbol, SuffixTreeEdge<TSymbol>>> _edges = new Lazy<SortedDictionary<TSymbol, SuffixTreeEdge<TSymbol>>>(() => new SortedDictionary<TSymbol, SuffixTreeEdge<TSymbol>>());

        public SortedDictionary<TSymbol, SuffixTreeEdge<TSymbol>> Edges => _edges.Value;
    }

    /// <summary>
    /// Represents edge of suffix tree.
    /// </summary>
    /// <typeparam name="TSymbol">Type of string symbol.</typeparam>
    public class SuffixTreeEdge<TSymbol>
    {
        public uint Start { get; internal set; }
        public uint Length { get; internal set; }
        public SuffixTreeNode<TSymbol> To { get; internal set; }
    }

    public class SuffixTreeNodeDebuggerProxy
    {
        public readonly GraphNode Node;

        public SuffixTreeNodeDebuggerProxy([NotNull] object node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            Node = BuildNode(node);
        }

        public static GraphNode BuildNode(object node, object[] text = null)
        {
            if (node == null)
                return null;

            var edges = (IEnumerable)node.GetType().GetProperty("Edges", BindingFlags.Instance | BindingFlags.Public).GetValue(node);

            var result = new GraphNode(Guid.NewGuid().ToString())
            {
                Content = ""
            };

            var children = new List<GraphEdge>();
            result.Edges = children;

            foreach (var edgePair in edges)
            {
                var edge = edgePair.GetType().GetProperty("Value", BindingFlags.Instance | BindingFlags.Public).GetValue(edgePair);

                var start = Convert.ToInt32(edge.GetType().GetProperty("Start", BindingFlags.Instance | BindingFlags.Public).GetValue(edge));
                var length = Convert.ToInt32(edge.GetType().GetProperty("Length", BindingFlags.Instance | BindingFlags.Public).GetValue(edge));
                var to = edge.GetType().GetProperty("To", BindingFlags.Instance | BindingFlags.Public).GetValue(edge);

                var edgeText = string.Join("", text?.Skip(start - 1).Take(length).Select(s => s.ToString()).ToArray() ?? new string[0]);

                children.Add(new GraphEdge
                {
                    IsDirected = true,
                    From = result,
                    Content = $"{start}:{length} {edgeText}",
                    To = BuildNode(to)
                });
            }

            return result;
        }
    }

    public class SuffixTreeDebuggerProxy
    {
        public readonly GraphNode Node;

        public SuffixTreeDebuggerProxy([NotNull] object tree)
        {
            if (tree == null) throw new ArgumentNullException(nameof(tree));

            var root = tree.GetType().GetField("Root", BindingFlags.Instance | BindingFlags.Public)?.GetValue(tree);
            var text = (IEnumerable) tree.GetType().GetField("_text", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(tree);

            Node = SuffixTreeNodeDebuggerProxy.BuildNode(root, text?.OfType<object>().ToArray());
        }
    }
}