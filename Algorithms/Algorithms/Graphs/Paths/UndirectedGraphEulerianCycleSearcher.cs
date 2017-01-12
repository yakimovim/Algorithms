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
            public bool Visited { get; set; }
            public long CycleIndex { get; set; }
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
            var adjacencyList = ConstructAdjacencyList(numberOfNodes, edgesProvider);

            var result = MarkCycles(adjacencyList);
            if (!result)
                return null;

            return ConstructEulerianCycle(adjacencyList);
        }

        private List<Edge>[] ConstructAdjacencyList(long numberOfNodes, Func<long, IEnumerable<long>> edgesProvider)
        {
            var adjacencyList = new List<Edge>[numberOfNodes];

            for (long node = 0; node < numberOfNodes; node++)
            {
                var currentNode = node;

                adjacencyList[currentNode] = new List<Edge>();

                var edges = edgesProvider(currentNode) ?? new long[0];

                foreach (var neighbor in edges.Where(n => n < currentNode))
                {
                    var edge = new Edge
                    {
                        End1 = currentNode,
                        End2 = neighbor
                    };

                    adjacencyList[currentNode].Add(edge);
                    adjacencyList[neighbor].Add(edge);
                }
            }

            return adjacencyList;
        }
        private bool MarkCycles(List<Edge>[] adjacencyList)
        {
            var finishedNodes = new bool[adjacencyList.LongLength];

            var cycleIndex = 0L;

            for (long node = 0; node < adjacencyList.LongLength; node++)
            {
                while (!finishedNodes[node])
                {
                    if (!MarkCycle(node, adjacencyList, cycleIndex++, finishedNodes))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool MarkCycle(long startNode, List<Edge>[] adjacencyList, long cycleIndex, bool[] finishedNodes)
        {
            var stack = new Stack<long>();
            stack.Push(startNode);

            while (stack.Count > 0)
            {
                var currentNode = stack.Pop();

                var nodeEdges = adjacencyList[currentNode];

                var nextEdge = nodeEdges.FirstOrDefault(e => e.CycleIndex == -1);
                if (nextEdge == null)
                    return false;

                nextEdge.CycleIndex = cycleIndex;

                if (nodeEdges.All(e => e.CycleIndex >= 0))
                {
                    finishedNodes[currentNode] = true;
                }

                currentNode = (nextEdge.End1 == currentNode)
                    ? nextEdge.End2
                    : nextEdge.End1;

                if (currentNode == startNode)
                    return true;
            }

            return false;
        }

        private Tuple<long, long>[] ConstructEulerianCycle(List<Edge>[] adjacencyList)
        {
            var eulerianCycleLength = adjacencyList.SelectMany(e => e).Count() / 2;

            var cycle = new List<Tuple<long, long>>();

            var currentNode = 0L;
            while (cycle.Count < eulerianCycleLength)
            {
                var nextEdge = adjacencyList[currentNode]
                    .Where(e => !e.Visited)
                    .MinBy(e => e.CycleIndex, Comparer<long>.Default);
                if (nextEdge == null)
                    return null;

                nextEdge.Visited = true;

                var nextNode = (nextEdge.End1 == currentNode)
                    ? nextEdge.End2
                    : nextEdge.End1;

                cycle.Add(Tuple.Create(currentNode, nextNode));

                currentNode = nextNode;
            }

            return cycle.ToArray();
        }
    }
}