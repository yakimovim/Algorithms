using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace EdlinSoftware.Algorithms.Graphs.Flows
{
    /// <summary>
    /// Represents implementation of Edmonds-Karp algorithm for max flow problem.
    /// </summary>
    /// <typeparam name="TNetworkNode">Type of network node.</typeparam>
    /// <typeparam name="TNetworkEdge">Type of network edges.</typeparam>
    public class EdmondsKarpMaxFlowFinder<TNetworkNode, TNetworkEdge>
        where TNetworkEdge : NetworkEdge<TNetworkNode>
    {
        private class NetworkEdgeWithFlow : NetworkEdge<TNetworkNode>
        {
            public TNetworkEdge InitialEdge { get; }

            public NetworkEdgeWithFlow(ulong capacity, [NotNull] TNetworkNode to, TNetworkEdge initialEdge) : base(capacity, to)
            {
                InitialEdge = initialEdge;
            }
        }

        private class ResidualNetworkEdge
        {
            public ResidualNetworkEdge(
                ulong capacity, 
                [NotNull] TNetworkNode to,
                [NotNull] NetworkEdgeWithFlow sourceEdge,
                bool sameDirection)
            {
                if (to == null) throw new ArgumentNullException(nameof(to));
                if (sourceEdge == null) throw new ArgumentNullException(nameof(sourceEdge));
                Capacity = capacity;
                To = to;
                SourceEdge = sourceEdge;
                SameDirection = sameDirection;
            }
            /// <summary>
            /// Gets capacity of the edge.
            /// </summary>
            public ulong Capacity { get; set; }
            /// <summary>
            /// Gets destination node of the edge.
            /// </summary>
            public TNetworkNode To { get; }
            /// <summary>
            /// Gets corresponding edge in residual network going in the opposite direction.
            /// </summary>
            public ResidualNetworkEdge ReverseEdge { get; set; }

            /// <summary>
            /// Gets corresponding edge of initial network.
            /// </summary>
            public NetworkEdgeWithFlow SourceEdge { get; }
            /// <summary>
            /// Gets if source edge goes in the same direction.
            /// </summary>
            public bool SameDirection { get; }
        }

        private TNetworkNode[] _sourceNodes;
        private HashSet<TNetworkNode> _sinkNodes;

        private Dictionary<TNetworkNode, List<NetworkEdgeWithFlow>> _networkOutEdges;

        private Dictionary<TNetworkNode, List<ResidualNetworkEdge>> _residualNetworkEdges;

        public ulong GetMaximumFlow(
            [NotNull] IEnumerable<TNetworkNode> sourceNodes,
            [NotNull] IEnumerable<TNetworkNode> sinkNodes,
            [NotNull] Func<TNetworkNode, IEnumerable<TNetworkEdge>> connectedNodesSelector)
        {
            if (sourceNodes == null) throw new ArgumentNullException(nameof(sourceNodes));
            if (sinkNodes == null) throw new ArgumentNullException(nameof(sinkNodes));
            if (connectedNodesSelector == null) throw new ArgumentNullException(nameof(connectedNodesSelector));

            ConstructNetwork(sourceNodes, sinkNodes, connectedNodesSelector);

            ConstructResidualNetwork();

            return GetMaximumFlow();
        }

        private void ConstructNetwork(IEnumerable<TNetworkNode> sourceNodes, IEnumerable<TNetworkNode> sinkNodes, Func<TNetworkNode, IEnumerable<TNetworkEdge>> connectedNodesSelector)
        {
            _sourceNodes = sourceNodes.ToArray();
            if(_sourceNodes.LongLength == 0L)
                throw new ArgumentException("There must be at least one source node.", nameof(sourceNodes));
            _sinkNodes = new HashSet<TNetworkNode>(sinkNodes);
            if (_sinkNodes.Count == 0)
                throw new ArgumentException("There must be at least one sink node.", nameof(sinkNodes));

            _networkOutEdges = new Dictionary<TNetworkNode, List<NetworkEdgeWithFlow>>();

            var visited = new HashSet<TNetworkNode>(_sourceNodes);
            var queue = new Queue<TNetworkNode>(_sourceNodes);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();

                var edges = connectedNodesSelector(node);

                foreach (var edge in edges)
                {
                    var networkEdgeWithFlow = new NetworkEdgeWithFlow(edge.Capacity, edge.To, edge);
                    _networkOutEdges.AddToDictionary(node, networkEdgeWithFlow);
                    if (!visited.Contains(edge.To))
                    {
                        queue.Enqueue(edge.To);
                        visited.Add(edge.To);
                    }
                }
            }
        }

        private void ConstructResidualNetwork()
        {
            _residualNetworkEdges = new Dictionary<TNetworkNode, List<ResidualNetworkEdge>>();

            foreach (var nodeEdges in _networkOutEdges)
            {
                var node = nodeEdges.Key;
                var edges = nodeEdges.Value;

                foreach (var edge in edges)
                {
                    var directEdge = new ResidualNetworkEdge(edge.Capacity, edge.To, edge, sameDirection: true);
                    var reverseEdge = new ResidualNetworkEdge(0L, node, edge, sameDirection: false);
                    directEdge.ReverseEdge = reverseEdge;
                    reverseEdge.ReverseEdge = directEdge;
                    _residualNetworkEdges.AddToDictionary(node, directEdge);
                    _residualNetworkEdges.AddToDictionary(edge.To, reverseEdge);
                }
            }
        }

        private ulong GetMaximumFlow()
        {
            var maxFlow = 0UL;

            while (true)
            {
                var pathInResidualNetwork = GetShortestPathFromSourceToSinkInResidualNetwork();
                if(pathInResidualNetwork.Count == 0)
                    break;
                var minimumCapacity = GetMinimumEdgeCapacity(pathInResidualNetwork);
                AdjustResidualNetwork(pathInResidualNetwork, minimumCapacity);
                maxFlow += minimumCapacity;
            }

            return maxFlow;
        }

        private List<ResidualNetworkEdge> GetShortestPathFromSourceToSinkInResidualNetwork()
        {
            var from = new Dictionary<TNetworkNode, ResidualNetworkEdge>();
            var visited = new HashSet<TNetworkNode>();

            var queueu = new Queue<TNetworkNode>();

            foreach (var node in _sourceNodes)
            {
                visited.Add(node);
                queueu.Enqueue(node);
            }

            while (queueu.Count > 0)
            {
                var node = queueu.Dequeue();

                if (_residualNetworkEdges.ContainsKey(node))
                {
                    var edges = _residualNetworkEdges[node].Where(e => e.Capacity > 0).Where(e => !visited.Contains(e.To));

                    foreach (var edge in edges)
                    {
                        visited.Add(edge.To);
                        from[edge.To] = edge;
                        queueu.Enqueue(edge.To);

                        if (_sinkNodes.Contains(edge.To))
                            return GetPath(edge.To, from);
                    }
                }
            }

            return new List<ResidualNetworkEdge>();
        }

        private List<ResidualNetworkEdge> GetPath(TNetworkNode node, Dictionary<TNetworkNode, ResidualNetworkEdge> @from)
        {
            var path = new List<ResidualNetworkEdge>();

            while (from.ContainsKey(node))
            {
                var edge = from[node];
                path.Add(edge);
                node = edge.ReverseEdge.To;
            }

            return path;
        }

        private ulong GetMinimumEdgeCapacity(List<ResidualNetworkEdge> pathInResidualNetwork)
        {
            return pathInResidualNetwork.Min(e => e.Capacity);
        }

        private void AdjustResidualNetwork(List<ResidualNetworkEdge> pathInResidualNetwork, ulong minimumCapacity)
        {
            foreach (var edge in pathInResidualNetwork)
            {
                edge.Capacity -= minimumCapacity;
                edge.ReverseEdge.Capacity += minimumCapacity;

                var sourceEdge = edge.SourceEdge;
                if (edge.SameDirection)
                {
                    sourceEdge.Flow += minimumCapacity;
                }
                else
                {
                    sourceEdge.Flow -= minimumCapacity;
                }

                sourceEdge.InitialEdge.Flow = sourceEdge.Flow;
            }
        }
    }

    /// <summary>
    /// Represents one directed edge of network.
    /// </summary>
    /// <typeparam name="TNetworkNode">Type of network node.</typeparam>
    public class NetworkEdge<TNetworkNode>
    {
        public NetworkEdge(ulong capacity, [NotNull] TNetworkNode to)
        {
            if (to == null) throw new ArgumentNullException(nameof(to));
            Capacity = capacity;
            To = to;
        }
        /// <summary>
        /// Gets capacity of the edge.
        /// </summary>
        public ulong Capacity { get; }
        /// <summary>
        /// Gets destination node of the edge.
        /// </summary>
        public TNetworkNode To { get; }
        /// <summary>
        /// Gets or sets flow for this edge.
        /// </summary>
        public ulong Flow { get; set; }
    }
}