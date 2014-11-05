using System;
using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.DataStructures.Graphs;
using EdlinSoftware.DataStructures.Graphs.Paths;

namespace EdlinSoftware.Algorithms.Graphs.Paths
{
    public class YenBellmanFordShortestPathSearcher : BellmanFordShortestPathSearcherBase
    {
        private IValuedEdge<long, double>[] _forwardEdges;
        private IValuedEdge<long, double>[] _backwardEdges;

        public override ISingleSourcePathsWithoutNegativeLoop<long, double, long> GetShortestPaths(long numberOfNodes, long initialNode, params IValuedEdge<long, double>[] edges)
        {
            if (numberOfNodes <= 0) throw new ArgumentOutOfRangeException("numberOfNodes");
            if (initialNode < 0 || initialNode >= numberOfNodes) throw new ArgumentOutOfRangeException("initialNode");

            InitializePathArrays(numberOfNodes, initialNode);

            SeparateForwardAndBackwardEdges(edges);

            RelaxEdgesRepeatedly();

            var hasNegativeLoop = CheckForNegativeLoop(edges);

            return new BellmanFordShortestPaths(initialNode, ShortestPathValues, PreviousNodeInShortestPath)
            {
                HasNegativeLoop = hasNegativeLoop
            };
        }

        private void SeparateForwardAndBackwardEdges(IEnumerable<IValuedEdge<long, double>> edges)
        {
            var forwardEdges = new LinkedList<IValuedEdge<long, double>>();
            var backwardEdges = new LinkedList<IValuedEdge<long, double>>();

            foreach (var edge in edges)
            {
                if (edge.End1 < edge.End2)
                    forwardEdges.AddLast(edge);
                else
                    backwardEdges.AddLast(edge);
            }

            _forwardEdges = forwardEdges.OrderBy(e => e.End1).ToArray();
            _backwardEdges = backwardEdges.OrderByDescending(e => e.End1).ToArray();
        }

        private void RelaxEdgesRepeatedly()
        {
            var size = ShortestPathValues.Length/2;

            for (long step = 0; step < size; step++)
            {
                foreach (var edge in _forwardEdges)
                {
                    RelaxEdge(edge);
                }
                foreach (var edge in _backwardEdges)
                {
                    RelaxEdge(edge);
                }
            }
        }
    }
}
