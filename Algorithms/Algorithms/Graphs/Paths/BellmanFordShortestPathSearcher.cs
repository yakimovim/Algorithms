using System;
using EdlinSoftware.DataStructures.Graphs;
using EdlinSoftware.DataStructures.Graphs.Paths;

namespace EdlinSoftware.Algorithms.Graphs.Paths
{
    public class BellmanFordShortestPathSearcher : BellmanFordShortestPathSearcherBase
    {
        public override ISingleSourcePathsWithoutNegativeLoop<long, double, long> GetShortestPaths(long numberOfNodes, long initialNode, params IValuedEdge<long, double>[] edges)
        {
            if (numberOfNodes <= 0) throw new ArgumentOutOfRangeException(nameof(numberOfNodes));
            if (initialNode < 0 || initialNode >= numberOfNodes) throw new ArgumentOutOfRangeException(nameof(initialNode));

            InitializePathArrays(numberOfNodes, initialNode);

            RelaxEdgesRepeatedly(edges);

            var hasNegativeLoop = CheckForNegativeLoop(edges);

            return new BellmanFordShortestPaths(initialNode, ShortestPathValues, PreviousNodeInShortestPath)
            {
                HasNegativeLoop = hasNegativeLoop
            };
        }

        private void RelaxEdgesRepeatedly(IValuedEdge<long, double>[] edges)
        {
            for (long step = 0; step < ShortestPathValues.LongLength; step++)
            {
                foreach (var edge in edges)
                {
                    RelaxEdge(edge);
                }
            }
        }
    }
}