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
        public readonly IReadOnlyList<TSymbol> Text;
        private readonly IComparer<TSymbol> _comparer;

        public readonly SuffixTreeNode<TSymbol> Root;

        public SuffixTree(
            IReadOnlyList<TSymbol> text, 
            TSymbol stopSymbol,
            [CanBeNull] IComparer<TSymbol> comparer = null)
        {
            Text = GetText(text, stopSymbol);
            _comparer = new StopSymbolFirstComparer<TSymbol>(comparer ?? Comparer<TSymbol>.Default, stopSymbol);
            Root = new SuffixTreeNode<TSymbol>(_comparer);

            for (uint i = 0; i < Text.Count; i++)
            {
                AddSuffix(Text, i);
            }
        }

        private static IReadOnlyList<TSymbol> GetText(IReadOnlyList<TSymbol> text, TSymbol stopSymbol)
        {
            if(text.Count == 0)
                return new List<TSymbol> { stopSymbol };

            return text[text.Count - 1].Equals(stopSymbol) ? text : new List<TSymbol>(text) { stopSymbol };
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
                    edge = new SuffixTreeEdge<TSymbol>
                    {
                        Start = index,
                        Length = restLength,
                        To = new SuffixTreeNode<TSymbol>(_comparer)
                        {
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
                            To = new SuffixTreeNode<TSymbol>(_comparer)
                        };

                        var edge2 = new SuffixTreeEdge<TSymbol>
                        {
                            Start = edge.Start + matchLength,
                            Length = edge.Length - matchLength,
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

    /// <summary>
    /// Represents node of suffix tree.
    /// </summary>
    /// <typeparam name="TSymbol">Type of string symbol.</typeparam>
    [DebuggerTypeProxy(typeof(SuffixTreeNodeDebuggerProxy))]
    public class SuffixTreeNode<TSymbol>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<SortedDictionary<TSymbol, SuffixTreeEdge<TSymbol>>> _edges;

        internal SuffixTreeNode([NotNull] IComparer<TSymbol> comparer)
        {
            if (comparer == null) throw new ArgumentNullException(nameof(comparer));
            _edges = new Lazy<SortedDictionary<TSymbol, SuffixTreeEdge<TSymbol>>>(() => new SortedDictionary<TSymbol, SuffixTreeEdge<TSymbol>>(comparer));
        }

        public SortedDictionary<TSymbol, SuffixTreeEdge<TSymbol>> Edges => _edges.Value;

        public uint? SuffixStart { get; set; }
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
        public GraphNode Node { get; }

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

                var edgeText = string.Join("", text?.Skip(start).Take(length).Select(s => s.ToString()).ToArray() ?? new string[0]);

                children.Add(new GraphEdge
                {
                    IsDirected = true,
                    From = result,
                    Content = $"{start}:{length} {edgeText}",
                    To = BuildNode(to, text)
                });
            }

            return result;
        }
    }

    public class SuffixTreeDebuggerProxy
    {
        public GraphNode Node { get; }

        public SuffixTreeDebuggerProxy([NotNull] object tree)
        {
            if (tree == null) throw new ArgumentNullException(nameof(tree));

            var root = tree.GetType().GetField("Root", BindingFlags.Instance | BindingFlags.Public)?.GetValue(tree);
            var text = (IEnumerable) tree.GetType().GetProperty("Text", BindingFlags.Instance | BindingFlags.Public)?.GetValue(tree);

            Node = SuffixTreeNodeDebuggerProxy.BuildNode(root, text?.OfType<object>().ToArray());
        }
    }
}