﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace EdlinSoftware.Algorithms.Graphs.Paths
{
    /// <summary>
    /// Represents searcher of Eulerian cycle in a directed graph.
    /// </summary>
    public class DirectedGraphEulerianCycleSearcher
    {
        private class Edge
        {
            public Edge()
            {
                CycleIndex = -1;
            }

            public long To { get; set; }

            public long CycleIndex { get; set; }
        }

        private class NodeEdges
        {
            private readonly LinkedList<Edge> _orderOfLeaving = new LinkedList<Edge>();
            private LinkedListNode<Edge> _lastLeavingEdgeNode;
            private LinkedListNode<Edge> _nextLeavingEdgeNode;

            public Edge[] Edges { get; set; }
            public long CurrentEdgeIndex { get; set; }

            public bool IsComplete => CurrentEdgeIndex >= Edges.LongLength;

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

        public Tuple<long, long>[] GetEulerianCycle(long numberOfNodes, Func<long, IEnumerable<long>> edgesSelector)
        {
            if (numberOfNodes == 0)
                return null;

            var adjuсencyList = CreateAdjuсencyList(numberOfNodes, edgesSelector);

            var result = MarkEdgesOrder(adjuсencyList);
            if(!result)
                return null;

            return ConstructEulerianCycle(adjuсencyList);
        }

        private NodeEdges[] CreateAdjuсencyList(long numberOfNodes, Func<long, IEnumerable<long>> edgesSelector)
        {
            var adjuсencyList = new NodeEdges[numberOfNodes];

            for (long node = 0; node < numberOfNodes; node++)
            {
                var edges = edgesSelector(node);

                adjuсencyList[node] = edges == null
                    ? new NodeEdges { Edges = new Edge[0] }
                    : new NodeEdges { Edges = edges.Select(n => new Edge { To = n }).ToArray() };
            }

            return adjuсencyList;
        }

        private bool MarkEdgesOrder(NodeEdges[] adjuсencyList)
        {
            long cycleIndex = 0;

            for (long node = 0; node < adjuсencyList.LongLength; node++)
            {
                while (!adjuсencyList[node].IsComplete)
                {
                    var result = MarkCycleFrom(node, adjuсencyList, cycleIndex);

                    if (!result)
                        return false;

                    cycleIndex++;
                }
            }

            return true;
        }

        private bool MarkCycleFrom(long initialNode, NodeEdges[] adjuсencyList, long cycleIndex)
        {
            var node = initialNode;

            while (true)
            {
                var nodeEdges = adjuсencyList[node];
                if (nodeEdges.IsComplete)
                    return false;

                var edge = nodeEdges.Edges[nodeEdges.CurrentEdgeIndex++];

                nodeEdges.AddLeavingEdge(edge, cycleIndex);

                node = edge.To;

                if (node == initialNode)
                    return true;
            }
        }

        private Tuple<long, long>[] ConstructEulerianCycle(NodeEdges[] adjuсencyList)
        {
            var numberOfEdges = adjuсencyList.SelectMany(n => n.Edges).Count();

            var cycle = new List<Tuple<long, long>>();

            long currentNode = 0;
            while (cycle.Count < numberOfEdges)
            {
                var nodeEdges = adjuсencyList[currentNode];

                var edge = nodeEdges.GetNextLeavingEdge();

                var node = edge.To;

                cycle.Add(Tuple.Create(currentNode, node));

                currentNode = node;
            }

            return cycle.ToArray();
        }
    }
}
