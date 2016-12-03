using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EdlinSoftware.Algorithms.Graphs.Search;

namespace EdlinSoftware.Algorithms.Graphs
{
    /// <summary>
    /// Represents builder of graph of strongly connected components.
    /// </summary>
    public class MetagraphBuilder
    {
        private Dictionary<long, HashSet<long>> _directEdges;
        private Dictionary<long, HashSet<long>> _reverseEdges;

        /// <summary>
        /// Builds graph of strongly connected components.
        /// </summary>
        /// <param name="numberOfNodes">Number of nodes in the initial directed graph.</param>
        /// <param name="connectedNodesSelector">Provider of outgoing edges for every node in the initial directed graph.</param>
        public Metagraph BuildMetagraph(long numberOfNodes, Func<long, IEnumerable<long>> connectedNodesSelector)
        {
            if (numberOfNodes < 0) throw new ArgumentOutOfRangeException(nameof(numberOfNodes));
            if (connectedNodesSelector == null) throw new ArgumentNullException(nameof(connectedNodesSelector));

            BuildEdges(numberOfNodes, connectedNodesSelector);

            var orderOfNodes = MakeOrderOfNodes(numberOfNodes);

            return BuildMetagraph(orderOfNodes);
        }
        private void BuildEdges(long numberOfNodes, Func<long, IEnumerable<long>> connectedNodesSelector)
        {
            _directEdges = new Dictionary<long, HashSet<long>>();
            _reverseEdges = new Dictionary<long, HashSet<long>>();

            for (long node = 0; node < numberOfNodes; node++)
            {
                foreach (var connectedNode in connectedNodesSelector(node))
                {
                    PutEdge(_directEdges, node, connectedNode);
                    PutEdge(_reverseEdges, connectedNode, node);
                }
            }
        }

        private void PutEdge(Dictionary<long, HashSet<long>> edges, long fromNode, long toNode)
        {
            if (edges.ContainsKey(fromNode) == false)
                edges[fromNode] = new HashSet<long>();
            edges[fromNode].Add(toNode);
        }

        private List<long> MakeOrderOfNodes(long numberOfNodes)
        {
            var visitedNodes = new HashSet<long>();
            var orderOfNodes = new List<long>();

            Func<long, IEnumerable<long>> connectedNodesProvider = node => _reverseEdges.ContainsKey(node) ? (IEnumerable<long>)_reverseEdges[node] : new long[0];

            var searcher = new DepthFirstSearcher<long>(InformationMoments.InformAfterChildren);

            for (long node = numberOfNodes - 1; node >= 0; node--)
            {
                if (visitedNodes.Contains(node) == false)
                {
                    searcher.Search(node, connectedNodesProvider, args =>
                    {
                        visitedNodes.Add(args.TargetNode);
                        orderOfNodes.Add(args.TargetNode);
                    });
                }
            }

            return orderOfNodes;
        }

        private Metagraph BuildMetagraph(List<long> orderOfNodes)
        {
            var visitedNodes = new HashSet<long>();

            var metagraphNodes = new Dictionary<long, HashSet<long>>();
            var metagraphEdges = new Dictionary<long, HashSet<long>>();
            var initialNodeToMetagraphNode = new Dictionary<long, long>();

            HashSet<long> currentComponent = null;
            long currentComponentIndex = -1L;

            Func<long, IEnumerable<long>> connectedNodesProvider = node => _directEdges.ContainsKey(node) ? (IEnumerable<long>)_directEdges[node] : new long[0];

            var searcher = new DepthFirstSearcher<long>();

            foreach (long node in orderOfNodes.Reverse<long>())
            {
                if (visitedNodes.Contains(node) == false)
                {
                    if (currentComponent != null)
                    {
                        metagraphNodes[metagraphNodes.Count] = currentComponent;
                    }

                    currentComponent = new HashSet<long>();
                    currentComponentIndex++;

                    var component = currentComponent;
                    var index = currentComponentIndex;
                    searcher.Search(node, connectedNodesProvider, args =>
                    {
                        initialNodeToMetagraphNode[args.TargetNode] = index;
                        visitedNodes.Add(args.TargetNode);
                        component.Add(args.TargetNode);
                    });
                }

                foreach (var neighborNode in connectedNodesProvider(node))
                {
                    if (initialNodeToMetagraphNode[neighborNode] != currentComponentIndex)
                    {
                        if (!metagraphEdges.ContainsKey(currentComponentIndex))
                            metagraphEdges[currentComponentIndex] = new HashSet<long>();
                        metagraphEdges[currentComponentIndex].Add(initialNodeToMetagraphNode[neighborNode]);
                    }
                }
            }

            if (currentComponent != null)
            {
                metagraphNodes[metagraphNodes.Count] = currentComponent;
            }

            return new Metagraph(metagraphNodes, metagraphEdges);
        }
    }

    /// <summary>
    /// Represents graph of strongly connected components.
    /// </summary>
    public class Metagraph
    {
        private readonly IReadOnlyDictionary<long, HashSet<long>> _metagraphNodes;
        private readonly IReadOnlyDictionary<long, HashSet<long>> _metagraphEdges;

        [DebuggerStepThrough]
        public Metagraph(IReadOnlyDictionary<long, HashSet<long>> metagraphNodes, IReadOnlyDictionary<long, HashSet<long>> metagraphEdges)
        {
            _metagraphNodes = metagraphNodes;
            _metagraphEdges = metagraphEdges;
        }

        /// <summary>
        /// Gets number of nodes in the metagraph.
        /// The same as number of connected components in the initial directed graph.
        /// </summary>
        public long NumberOfNodes
        {
            [DebuggerStepThrough]
            get { return _metagraphNodes.Count; }
        }

        /// <summary>
        /// Gets provider of edges for node in metagraph.
        /// </summary>
        public Func<long, IEnumerable<long>> EdgesProvider
        {
            get { return (node) => _metagraphEdges.ContainsKey(node) ? (IEnumerable<long>) _metagraphEdges[node] : new long[0]; }
        }

        /// <summary>
        /// Returns all nodes of initial graph for given node of metagraph.
        /// </summary>
        public HashSet<long> ComponentNodes(long metagraphNode)
        {
            if(metagraphNode < 0 || metagraphNode >= _metagraphNodes.Count)
                throw new ArgumentOutOfRangeException(nameof(metagraphNode));

            return _metagraphNodes[metagraphNode];
        }
    }
}