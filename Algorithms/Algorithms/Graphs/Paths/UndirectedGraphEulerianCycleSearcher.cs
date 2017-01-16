using System;
using System.Collections.Generic;
using System.Linq;

namespace EdlinSoftware.Algorithms.Graphs.Paths
{
    /// <summary>
    /// Represents searcher of Eulerian cycle in an undirected graph.
    /// </summary>
    public class UndirectedGraphEulerianCycleSearcher
    {
        private class Edge
        {
            public Edge()
            {
                CycleIndex = -1;
            }

            public long End1 { get; set; }
            public long End2 { get; set; }
            public long CycleIndex { get; set; }
        }

        private class NodeEdges
        {
            private readonly LinkedList<Edge> _orderOfLeaving = new LinkedList<Edge>();
            private LinkedListNode<Edge> _lastLeavingEdgeNode;
            private LinkedListNode<Edge> _nextLeavingEdgeNode;

            public HashSet<Edge> NotVisitedEdges { get; }

            public bool IsComplete => NotVisitedEdges.Count == 0;

            public NodeEdges()
            {
                NotVisitedEdges = new HashSet<Edge>();
            }

            public void AddLeavingEdge(Edge edge, long cycleIndex)
            {
                edge.CycleIndex = cycleIndex;

                if (_lastLeavingEdgeNode == null || _lastLeavingEdgeNode.Value.CycleIndex < cycleIndex)
                {
                    _lastLeavingEdgeNode = _orderOfLeaving.AddFirst(edge);
                }
                else
                {
                    _lastLeavingEdgeNode = _orderOfLeaving.AddAfter(_lastLeavingEdgeNode, edge);
                }
            }

            public Edge GetNextLeavingEdge()
            {
                if (_nextLeavingEdgeNode == null)
                {
                    _nextLeavingEdgeNode = _orderOfLeaving.First;
                }

                var result = _nextLeavingEdgeNode.Value;

                _nextLeavingEdgeNode = _nextLeavingEdgeNode.Next;

                return result;
            }
        }

        /// <summary>
        /// Returns Eulerian cycle for an undirected graph.
        /// </summary>
        /// <param name="numberOfNodes">Number of nodes in the undirected graph.</param>
        /// <param name="edgesProvider">Provider of edges for any node.</param>
        /// <returns>Array of tuples corresponding to edges of Eulerian cycle. Null if there is no Eulerian cycle.</returns>
        public Tuple<long, long>[] GetEulerianCycle(
            long numberOfNodes,
            Func<long, IEnumerable<long>> edgesProvider)
        {
            if (numberOfNodes <= 0)
                return null;

            var adjacencyList = ConstructAdjacencyList(numberOfNodes, edgesProvider);

            var numberOfEdges = adjacencyList.Select(ne => ne.NotVisitedEdges.Count).Sum() / 2;

            var result = MarkCycles(adjacencyList);
            if (!result)
                return null;

            return ConstructEulerianCycle(numberOfEdges, adjacencyList);
        }

        private NodeEdges[] ConstructAdjacencyList(long numberOfNodes, Func<long, IEnumerable<long>> edgesProvider)
        {
            var adjacencyList = new NodeEdges[numberOfNodes];

            for (long node = 0; node < numberOfNodes; node++)
            {
                var currentNode = node;

                adjacencyList[currentNode] = new NodeEdges();

                var edges = edgesProvider(currentNode) ?? new long[0];

                foreach (var neighbor in edges.Where(n => n <= currentNode))
                {
                    var edge = new Edge
                    {
                        End1 = currentNode,
                        End2 = neighbor
                    };

                    adjacencyList[currentNode].NotVisitedEdges.Add(edge);
                    adjacencyList[neighbor].NotVisitedEdges.Add(edge);
                }
            }

            return adjacencyList;
        }
        private bool MarkCycles(NodeEdges[] adjacencyList)
        {
            var cycleIndex = 0L;

            for (long node = 0; node < adjacencyList.LongLength; node++)
            {
                var nodeEdges = adjacencyList[node];

                while (!nodeEdges.IsComplete)
                {
                    if (!MarkCycle(node, adjacencyList, cycleIndex++))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool MarkCycle(long startNode, NodeEdges[] adjacencyList, long cycleIndex)
        {
            var currentNode = startNode;

            while (true)
            {
                var nodeEdges = adjacencyList[currentNode];
                if (nodeEdges.IsComplete)
                    return false;

                var edge = nodeEdges.NotVisitedEdges.First();

                nodeEdges.NotVisitedEdges.Remove(edge);

                nodeEdges.AddLeavingEdge(edge, cycleIndex);

                currentNode = (edge.End1 == currentNode)
                    ? edge.End2
                    : edge.End1;

                adjacencyList[currentNode].NotVisitedEdges.Remove(edge);

                if (currentNode == startNode)
                {
                    return true;
                }
            }
        }

        private Tuple<long, long>[] ConstructEulerianCycle(int numberOfEdges, NodeEdges[] adjacencyList)
        {
            var eulerianCycleLength = numberOfEdges;

            var cycle = new List<Tuple<long, long>>();

            var currentNode = 0L;
            while (cycle.Count < eulerianCycleLength)
            {
                var nodeEdges = adjacencyList[currentNode];

                var edge = nodeEdges.GetNextLeavingEdge();

                var node = edge.End1 == currentNode
                    ? edge.End2
                    : edge.End1;

                cycle.Add(Tuple.Create(currentNode, node));

                currentNode = node;
            }

            return cycle.ToArray();
        }
    }
}