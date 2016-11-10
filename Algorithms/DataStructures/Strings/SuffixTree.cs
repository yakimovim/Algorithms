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

        public readonly SuffixTreeNode<TSymbol> Root;

        public SuffixTree([NotNull] IReadOnlyList<TSymbol> text, [NotNull] SuffixTreeNode<TSymbol> root)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            if (root == null) throw new ArgumentNullException(nameof(root));
            Text = text;
            Root = root;
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

        public SuffixTreeEdge<TSymbol> InEdge { get; internal set; }

        public SortedDictionary<TSymbol, SuffixTreeEdge<TSymbol>> Edges => _edges.Value;

        public uint StringDepth { get; internal set; }

        public uint? SuffixStart { get; internal set; }
    }

    /// <summary>
    /// Represents edge of suffix tree.
    /// </summary>
    /// <typeparam name="TSymbol">Type of string symbol.</typeparam>
    public class SuffixTreeEdge<TSymbol>
    {

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SuffixTreeNode<TSymbol> _to;

        public uint Start { get; internal set; }
        public uint Length { get; internal set; }
        public SuffixTreeNode<TSymbol> From { get; internal set; }

        public SuffixTreeNode<TSymbol> To
        {
            [DebuggerStepThrough]
            get {  return _to; }

            [DebuggerStepThrough]
            internal set
            {
                _to = value;
                if (_to != null)
                {
                    _to.InEdge = this;
                }
            }
        }
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