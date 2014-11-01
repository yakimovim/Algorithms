using System;
using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.DataStructures.Graphs;
using EdlinSoftware.DataStructures.Heaps;

namespace EdlinSoftware.Algorithms.Graphs.MinimumSpanningTree
{
    /// <summary>
    /// Represents Prim's algorithm of minimum spanning tree search.
    /// </summary>
    public class PrimAlgorithm : IMinimumSpanningTreeAlgorithm
    {
        private class EdgeComparer : IComparer<IValuedEdge<long, long>>
        {
            public int Compare(IValuedEdge<long, long> x, IValuedEdge<long, long> y)
            {
                return x.Value.CompareTo(y.Value);
            }
        }

        private long _numberOfNodesInGraph;
        private Dictionary<long, List<IValuedEdge<long, long>>> _connectivityInfo;
        private HashSet<long> _visitedNodes;
        private UniqueHeap<IValuedEdge<long, long>, long> _nodesHeap;

        public IEnumerable<IValuedEdge<long, long>> GetMinimumSpanningTree(long numberOfNodesInGraph, IEnumerable<IValuedEdge<long, long>> edgesOfGraph)
        {
            _numberOfNodesInGraph = numberOfNodesInGraph;

            _connectivityInfo = GetConnectivityInfo(edgesOfGraph);

            return GetMinimumSpanningTree();
        }

        private Dictionary<long, List<IValuedEdge<long, long>>> GetConnectivityInfo(IEnumerable<IValuedEdge<long, long>> edgesOfGraph)
        {
            var connectivityInfo = new Dictionary<long, List<IValuedEdge<long, long>>>();

            if (edgesOfGraph != null)
            {
                foreach (var edge in edgesOfGraph)
                {
                    AddEdge(connectivityInfo, edge);
                }
            }

            return connectivityInfo;
        }

        private void AddEdge(Dictionary<long, List<IValuedEdge<long, long>>> connectivityInfo, IValuedEdge<long, long> edge)
        {
            AddEdgeToNode(connectivityInfo, edge.End1, edge);
            AddEdgeToNode(connectivityInfo, edge.End2, edge);
        }

        private static void AddEdgeToNode(Dictionary<long, List<IValuedEdge<long, long>>> connectivityInfo, long node, IValuedEdge<long, long> edge)
        {
            if (connectivityInfo.ContainsKey(node) == false)
                connectivityInfo[node] = new List<IValuedEdge<long, long>>();
            connectivityInfo[node].Add(edge);
        }

        private IEnumerable<IValuedEdge<long, long>> GetMinimumSpanningTree()
        {
            if (_numberOfNodesInGraph <= 1)
                return new IValuedEdge<long, long>[0];

            _visitedNodes = new HashSet<long>(new long[] { 1 });
            _nodesHeap = new UniqueHeap<IValuedEdge<long, long>, long>(new EdgeComparer());

            RecalculateHeapForNode(1);

            var minimumSpanningTree = new List<IValuedEdge<long, long>>();

            while (_visitedNodes.Count < _numberOfNodesInGraph)
            {
                var nodeInfo = _nodesHeap.ExtractMinElement();

                var nextNode = nodeInfo.Value;
                
                _visitedNodes.Add(nextNode);

                RecalculateHeapForNode(nextNode);

                minimumSpanningTree.Add(nodeInfo.Key);
            }

            return minimumSpanningTree;
        }

        private void RecalculateHeapForNode(long node)
        {
            var edgesOfNode = _connectivityInfo[node];

            var nodesToRecalculate = edgesOfNode
                .Select(e => GoesFrom(e, node))
                .Where(n => _visitedNodes.Contains(n) == false)
                .ToArray();

            foreach (var heapNode in nodesToRecalculate)
            {
                RecalculateNodeInHeap(heapNode);
            }
        }

        private long GoesFrom(IValuedEdge<long, long> edge, long node)
        {
            if (edge.End1 == node)
                return edge.End2;
            if (edge.End2 == node)
                return edge.End1;
            throw new InvalidOperationException();
        }

        private void RecalculateNodeInHeap(long node)
        {
            _nodesHeap.Remove(node);

            var edgesOfNode = _connectivityInfo[node]
                .Where(e => _visitedNodes.Contains(GoesFrom(e, node)))
                .ToArray();

            var minCost = long.MaxValue;
            IValuedEdge<long, long> minEdge = null;

            foreach (var edge in edgesOfNode)
            {
                if (edge.Value < minCost)
                {
                    minCost = edge.Value;
                    minEdge = edge;
                }
            }

            _nodesHeap.Add(minEdge, node);
        }
    }
}
