using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EdlinSoftware.DataStructures.Graphs
{
    /// <summary>
    /// Represents edge of graph.
    /// </summary>
    /// <typeparam name="TNode">Type of node.</typeparam>
    public interface IEdge<TNode>
    {
        /// <summary>
        /// Gets or sets the first node of the edge.
        /// </summary>
        TNode End1 { get; set; }

        /// <summary>
        /// Gets or sets the second node of the edge.
        /// </summary>
        TNode End2 { get; set; }

        /// <summary>
        /// Check if the edge goes from given node.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>True, if the edge goes from given node. False, otherwise.</returns>
        bool GoesFrom(TNode node);

        /// <summary>
        /// Check if the edge goes to given node.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>True, if the edge goes to given node. False, otherwise.</returns>
        bool GoesTo(TNode node);

        /// <summary>
        /// Check if the edge goes from one node to the other.
        /// </summary>
        /// <param name="from">Source node.</param>
        /// <param name="to">Target node.</param>
        /// <returns>True, if the edge goes from one node to the other. False, otherwise.</returns>
        bool GoesFromTo(TNode from, TNode to);
    }

    /// <summary>
    /// Represents edge with some associated value.
    /// </summary>
    /// <typeparam name="TNode">Type of node.</typeparam>
    /// <typeparam name="TValue">Type of edge value.</typeparam>
    public interface IValuedEdge<TNode, TValue> : IEdge<TNode>
    {
        /// <summary>
        /// Gets or sets value of the edge.
        /// </summary>
        TValue Value { get; set; }
    }

    /// <summary>
    /// Base class of graph edges.
    /// </summary>
    /// <typeparam name="TNode">Type of node.</typeparam>
    public abstract class Edge<TNode> : IEdge<TNode>
    {
        protected readonly IEqualityComparer<TNode> NodesComparer;

        [DebuggerStepThrough]
        protected Edge(IEqualityComparer<TNode> nodesComparer = null)
        {
            NodesComparer = nodesComparer;
        }

        /// <summary>
        /// Gets or sets the first node of the edge.
        /// </summary>
        public TNode End1 { get; set; }

        /// <summary>
        /// Gets or sets the second node of the edge.
        /// </summary>
        public TNode End2 { get; set; }
        /// <summary>
        /// Check if the edge goes from given node.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>True, if the edge goes from given node. False, otherwise.</returns>
        public abstract bool GoesFrom(TNode node);

        /// <summary>
        /// Check if the edge goes to given node.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>True, if the edge goes to given node. False, otherwise.</returns>
        public abstract bool GoesTo(TNode node);

        /// <summary>
        /// Check if the edge goes from one node to the other.
        /// </summary>
        /// <param name="from">Source node.</param>
        /// <param name="to">Target node.</param>
        /// <returns>True, if the edge goes from one node to the other. False, otherwise.</returns>
        public abstract bool GoesFromTo(TNode from, TNode to);
    }

    /// <summary>
    /// Represents undirected edge.
    /// </summary>
    /// <typeparam name="TNode">Type of node.</typeparam>
    public class UndirectedEdge<TNode> : Edge<TNode>
    {
        [DebuggerStepThrough]
        public UndirectedEdge(IEqualityComparer<TNode> nodesComparer = null)
            : base(nodesComparer)
        {}

        /// <summary>
        /// Check if the edge goes from given node.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>True, if the edge goes from given node. False, otherwise.</returns>
        public override bool GoesFrom(TNode node)
        {
            if(ReferenceEquals(node, null)) return false;

            if (NodesComparer == null)
            {
                return ReferenceEquals(node, End1)
                    || ReferenceEquals(node, End2);
            }
            
            return NodesComparer.Equals(node, End1)
                   || NodesComparer.Equals(node, End2);
        }

        /// <summary>
        /// Check if the edge goes to given node.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>True, if the edge goes to given node. False, otherwise.</returns>
        public override bool GoesTo(TNode node)
        {
            return GoesFrom(node);
        }

        /// <summary>
        /// Check if the edge goes from one node to the other.
        /// </summary>
        /// <param name="from">Source node.</param>
        /// <param name="to">Target node.</param>
        /// <returns>True, if the edge goes from one node to the other. False, otherwise.</returns>
        public override bool GoesFromTo(TNode from, TNode to)
        {
            if (ReferenceEquals(from, null) || ReferenceEquals(to, null)) return false;

            if (NodesComparer == null)
            {
                return (ReferenceEquals(from, End1) && ReferenceEquals(to, End2))
                    || (ReferenceEquals(from, End2) && ReferenceEquals(to, End1));
            }
            
            return (NodesComparer.Equals(@from, End1) && NodesComparer.Equals(to, End2))
                   || (NodesComparer.Equals(@from, End2) && NodesComparer.Equals(to, End1));
        }
    }

    /// <summary>
    /// Represents directed edge.
    /// </summary>
    /// <typeparam name="TNode">Type of node.</typeparam>
    public class DirectedEdge<TNode> : Edge<TNode>
    {
        [DebuggerStepThrough]
        public DirectedEdge(IEqualityComparer<TNode> nodesComparer = null)
            : base(nodesComparer)
        {}

        /// <summary>
        /// Check if the edge goes from given node.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>True, if the edge goes from given node. False, otherwise.</returns>
        public override bool GoesFrom(TNode node)
        {
            if (ReferenceEquals(node, null)) return false;

            if (NodesComparer == null)
            {
                return ReferenceEquals(node, End1);
            }
            
            return NodesComparer.Equals(node, End1);
        }

        /// <summary>
        /// Check if the edge goes to given node.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>True, if the edge goes to given node. False, otherwise.</returns>
        public override bool GoesTo(TNode node)
        {
            if (ReferenceEquals(node, null)) return false;

            if (NodesComparer == null)
            {
                return ReferenceEquals(node, End2);
            }
            
            return NodesComparer.Equals(node, End2);
        }

        /// <summary>
        /// Check if the edge goes from one node to the other.
        /// </summary>
        /// <param name="from">Source node.</param>
        /// <param name="to">Target node.</param>
        /// <returns>True, if the edge goes from one node to the other. False, otherwise.</returns>
        public override bool GoesFromTo(TNode from, TNode to)
        {
            return GoesFrom(from) && GoesTo(to);
        }
    }

    /// <summary>
    /// Represents edge with some value.
    /// </summary>
    /// <typeparam name="TNode">Type of node.</typeparam>
    /// <typeparam name="TValue">Type of value.</typeparam>
    public class ValuedEdge<TNode, TValue> : IValuedEdge<TNode, TValue>
    {
        private readonly IEdge<TNode> _edge;

        public static ValuedEdge<TNode, TValue> Undirected(IEqualityComparer<TNode> nodesComparer = null,
            Action<IValuedEdge<TNode, TValue>> initializer = null)
        {
            var valuedEdge = new ValuedEdge<TNode,TValue>(new UndirectedEdge<TNode>(nodesComparer));

            if (initializer != null)
            {
                initializer(valuedEdge);
            }

            return valuedEdge;
        }

        public static ValuedEdge<TNode, TValue> Directed(IEqualityComparer<TNode> nodesComparer = null,
            Action<IValuedEdge<TNode, TValue>> initializer = null)
        {
            var valuedEdge = new ValuedEdge<TNode, TValue>(new DirectedEdge<TNode>(nodesComparer));

            if (initializer != null)
            {
                initializer(valuedEdge);
            }

            return valuedEdge;
        }

        public static ValuedEdge<TNode, TValue> FromEdge(IEdge<TNode> edge)
        {
            return new ValuedEdge<TNode, TValue>(edge);
        }

        [DebuggerStepThrough]
        protected ValuedEdge(IEdge<TNode> edge)
        {
            if (edge == null) throw new ArgumentNullException("edge");
            _edge = edge;
        }

        /// <summary>
        /// Gets or sets value of the edge.
        /// </summary>
        public TValue Value { get; set; }

        /// <summary>
        /// Gets or sets the first node of the edge.
        /// </summary>
        public TNode End1
        {
            [DebuggerStepThrough]
            get { return _edge.End1; }
            [DebuggerStepThrough]
            set { _edge.End1 = value; }
        }

        /// <summary>
        /// Gets or sets the second node of the edge.
        /// </summary>
        public TNode End2
        {
            [DebuggerStepThrough]
            get { return _edge.End2; }
            [DebuggerStepThrough]
            set { _edge.End2 = value; }
        }

        /// <summary>
        /// Check if the edge goes from given node.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>True, if the edge goes from given node. False, otherwise.</returns>
        public bool GoesFrom(TNode node)
        {
            return _edge.GoesFrom(node);
        }

        /// <summary>
        /// Check if the edge goes to given node.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>True, if the edge goes to given node. False, otherwise.</returns>
        public bool GoesTo(TNode node)
        {
            return _edge.GoesTo(node);
        }

        /// <summary>
        /// Check if the edge goes from one node to the other.
        /// </summary>
        /// <param name="from">Source node.</param>
        /// <param name="to">Target node.</param>
        /// <returns>True, if the edge goes from one node to the other. False, otherwise.</returns>
        public bool GoesFromTo(TNode from, TNode to)
        {
            return _edge.GoesFromTo(from, to);
        }
    }

    /// <summary>
    /// Provides additional operations for edges.
    /// </summary>
    public static class EdgeExtender
    {
        /// <summary>
        /// Reverses direction of edge.
        /// </summary>
        /// <param name="edge">Edge to reverse.</param>
        public static void Reverse<TNode>(this IEdge<TNode> edge)
        {
            var edgeEnd = edge.End1;
            edge.End1 = edge.End2;
            edge.End2 = edgeEnd;
        }
    }
}
