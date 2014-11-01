using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdlinSoftware.DataStructures.Graphs
{
    /// <summary>
    /// Represents node (vertex) of graph.
    /// </summary>
    public class Node<TNode, TEdge>
        where TNode : Node<TNode, TEdge>
        where TEdge : IEdge<TNode>
    {
        private readonly IDictionary<TNode, IList<TEdge>> _edges = new Dictionary<TNode, IList<TEdge>>();

        /// <summary>
        /// Gets list of all edges for this node.
        /// </summary>
        public virtual IEnumerable<TEdge> AllEdges
        {
            [DebuggerStepThrough]
            get { return _edges.Values.SelectMany(v => v); }
        }

        /// <summary>
        /// Gets list of outgoing edges for this node.
        /// </summary>
        public virtual IEnumerable<TEdge> OutgoingEdges
        {
            [DebuggerStepThrough]
            get { return AllEdges.Where(e => e.GoesFrom(this as TNode)); }
        }

        /// <summary>
        /// Adds edge to the node.
        /// </summary>
        /// <param name="edge">Edge.</param>
        public virtual void AddEdge(TEdge edge)
        {
            if (edge == null) throw new ArgumentNullException("edge");

            var other = GetOtherNode(edge);
            if (other == null)
            { throw new ArgumentException("The edge is not connected to this node.", "edge"); }

            if (_edges.ContainsKey(other) == false)
            { _edges[other] = new List<TEdge>(); }

            _edges[other].Add(edge);
        }

        /// <summary>
        /// Gets node connected to this by the edge.
        /// </summary>
        /// <param name="edge">Edge.</param>
        /// <returns>Node connected to this by the edge. Null if the edge is not connected to this node.</returns>
        private TNode GetOtherNode(TEdge edge)
        {
            if (edge.End1 == this) return edge.End2;
            if (edge.End2 == this) return edge.End1;
            return null;
        }

        /// <summary>
        /// Removes edge from the node.
        /// </summary>
        /// <param name="edge">Edge.</param>
        /// <returns>True, if edge was removed. False, otherwise.</returns>
        public virtual bool RemoveEdge(TEdge edge)
        {
            if (edge == null) return false;

            var other = GetOtherNode(edge);
            if (other == null) return false;
            if (_edges.ContainsKey(other) == false) return false;

            var result = _edges[other].Remove(edge);

            if (_edges[other].Count == 0)
            { _edges.Remove(other); }

            return result;
        }

        /// <summary>
        /// Gets all edges from this node to another node.
        /// </summary>
        /// <param name="node">Target node.</param>
        public IEnumerable<TEdge> GetEdgesTo(TNode node)
        {
            if (_edges.ContainsKey(node) == false)
                return new TEdge[0];

            return _edges[node];
        }

        /// <summary>
        /// Gets collection of nodes connected to this node by outgoing edges.
        /// </summary>
        public IEnumerable<TNode> GetOutgoingNodes()
        {
            return _edges.Keys.Where(node => _edges[node].Any(e => e.GoesFrom(this as TNode)));
        }
    }

    /// <summary>
    /// Represents node (vertex) of graph with some associated value.
    /// </summary>
    public class ValuedNode<TNode, TEdge, TValue> : Node<TNode, TEdge>
        where TNode : ValuedNode<TNode, TEdge, TValue>
        where TEdge : IEdge<TNode>
    {
        /// <summary>
        /// Gets or sets value of the node.
        /// </summary>
        public TValue Value { get; set; }
    }
}
