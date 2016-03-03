using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace EdlinSoftware.DataStructures.Graphs
{
    /// <summary>
    /// Represents graph with separate nodes and edges.
    /// </summary>
    /// <typeparam name="TNode">Type of node.</typeparam>
    /// <typeparam name="TEdge">Type of edge.</typeparam>
    public class Graph<TNode, TEdge>
        where TNode : Node<TNode, TEdge>
        where TEdge : IEdge<TNode>
    {
        private readonly HashSet<TNode> _nodes = new HashSet<TNode>();
        private readonly HashSet<TEdge> _edges = new HashSet<TEdge>();

        /// <summary>
        /// Gets list of nodes (vertices) in the graph.
        /// </summary>
        public IEnumerable<TNode> Nodes
        {
            [DebuggerStepThrough]
            get { return _nodes; }
        }

        /// <summary>
        /// Gets number of nodes in the graph.
        /// </summary>
        public long NodesCount
        {
            [DebuggerStepThrough]
            get { return _nodes.Count; }
        }

        /// <summary>
        /// Gets list of edges in the graph.
        /// </summary>
        public IEnumerable<TEdge> Edges
        {
            [DebuggerStepThrough]
            get { return _edges; }
        }

        /// <summary>
        /// Gets number of edges in the graph.
        /// </summary>
        public long EdgesCount
        {
            [DebuggerStepThrough]
            get { return _edges.Count; }
        }

        /// <summary>
        /// Add node to the graph. Its edges are not added.
        /// </summary>
        /// <param name="node">Node.</param>
        public void AddNode(TNode node)
        {
            if (node == null) throw new ArgumentNullException(nameof(node));

            _nodes.Add(node);
        }

        /// <summary>
        /// Removes node and all adjucent edges from the graph.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <returns>True, if node was removed. False, otherwise.</returns>
        public bool RemoveNode(TNode node)
        {
            if (_nodes.Contains(node) == false) { return false; }

            foreach (var nodeEdge in node.AllEdges)
            {
                _edges.Remove(nodeEdge);
            }

            _nodes.Remove(node);

            return true;
        }

        /// <summary>
        /// Add edge to the graph. Checks if its ends are in the graph.
        /// </summary>
        /// <param name="edge">Edge.</param>
        public void AddEdge(TEdge edge)
        {
            if (edge == null) throw new ArgumentNullException(nameof(edge));

            if (_nodes.Contains(edge.End1) == false)
                throw new ArgumentException("End1 of edge is not in the graph.");
            if (_nodes.Contains(edge.End2) == false)
                throw new ArgumentException("End2 of edge is not in the graph.");

            _edges.Add(edge);
        }

        /// <summary>
        /// Removes edge from graph.
        /// </summary>
        /// <param name="edge">Edge.</param>
        /// <returns>True, if edge was removed. False, otherwise.</returns>
        public bool RemoveEdge(TEdge edge)
        {
            if (_edges.Contains(edge) == false) { return false; }

            edge.End1.RemoveEdge(edge);
            edge.End2.RemoveEdge(edge);

            _edges.Remove(edge);

            return true;
        }
    }

    /// <summary>
    /// Represents graph where nodes are just numbers.
    /// </summary>
    /// <typeparam name="TEdge">Type of edge.</typeparam>
    public class Graph<TEdge>
        where TEdge : IEdge<long>
    {
        private readonly HashSet<TEdge> _edges = new HashSet<TEdge>();
        private readonly Dictionary<long, IList<TEdge>> _nodeEdges = new Dictionary<long, IList<TEdge>>();

        public Graph(long numberOfNodes)
        {
            NodesCount = numberOfNodes;
        }

        /// <summary>
        /// Gets number of nodes in the graph.
        /// </summary>
        public long NodesCount { get; }

        /// <summary>
        /// Gets list of edges in the graph.
        /// </summary>
        public IEnumerable<TEdge> Edges
        {
            [DebuggerStepThrough]
            get { return _edges; }
        }

        /// <summary>
        /// Gets number of edges in the graph.
        /// </summary>
        public long EdgesCount
        {
            [DebuggerStepThrough]
            get { return _edges.Count; }
        }


        /// <summary>
        /// Add edge to the graph. Checks if its ends are in the graph.
        /// </summary>
        /// <param name="edge">Edge.</param>
        public void AddEdge(TEdge edge)
        {
            if (edge == null) throw new ArgumentNullException(nameof(edge));

            CheckNode(edge.End1, "edge", "End1 of edge is not in the graph.");
            CheckNode(edge.End2, "edge", "End2 of edge is not in the graph.");

            _edges.Add(edge);

            if (_nodeEdges.ContainsKey(edge.End1) == false)
            { _nodeEdges[edge.End1] = new List<TEdge>(); }

            _nodeEdges[edge.End1].Add(edge);

            if (_nodeEdges.ContainsKey(edge.End2) == false)
            { _nodeEdges[edge.End2] = new List<TEdge>(); }

            _nodeEdges[edge.End2].Add(edge);
        }

        /// <summary>
        /// Checks if node is in the graph.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <param name="parameterName">Parameter name for exception.</param>
        /// <param name="message">Message for exception.</param>
        private void CheckNode(long node, string parameterName = null, string message = null)
        {
            if (node < 0 || node >= NodesCount)
                throw new ArgumentException(message ?? "Node is not in the graph", parameterName ?? "node");
        }

        /// <summary>
        /// Removes edge from graph.
        /// </summary>
        /// <param name="edge">Edge.</param>
        /// <returns>True, if edge was removed. False, otherwise.</returns>
        public bool RemoveEdge(TEdge edge)
        {
            if (_edges.Contains(edge) == false) { return false; }

            _edges.Remove(edge);

            _nodeEdges[edge.End1].Remove(edge);
            if (_nodeEdges[edge.End1].Count == 0)
            { _nodeEdges.Remove(edge.End1); }

            _nodeEdges[edge.End2].Remove(edge);
            if (_nodeEdges[edge.End2].Count == 0)
            { _nodeEdges.Remove(edge.End2); }

            return true;
        }

        /// <summary>
        /// Returns all edges connected to the node.
        /// </summary>
        /// <param name="node">Node.</param>
        public IEnumerable<TEdge> GetAllEdges(long node)
        {
            return _nodeEdges.ContainsKey(node)
                ? _nodeEdges[node]
                : new TEdge[0];
        }

        /// <summary>
        /// Returns all outgoing edges for the node.
        /// </summary>
        /// <param name="node">Node.</param>
        public IEnumerable<TEdge> GetOutgoingEdges(long node)
        {
            return GetAllEdges(node).Where(e => e.GoesFrom(node));
        }

        /// <summary>
        /// Gets all edges from this node to another node.
        /// </summary>
        /// <param name="from">Source node.</param>
        /// <param name="to">Target node.</param>
        public IEnumerable<TEdge> GetEdgesBetween(long from, long to)
        {
            CheckNode(from, "from");
            CheckNode(from, "to");

            return GetOutgoingEdges(from).Where(e => e.GoesTo(to));
        }

        /// <summary>
        /// Gets collection of nodes connected to this node by outgoing edges.
        /// </summary>
        public IEnumerable<long> GetOutgoingNodes(long node)
        {
            CheckNode(node);

            return GetOutgoingEdges(node).Select(e => e.End1 == node ? e.End2 : e.End1);
        }
    }
}
