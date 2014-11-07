using System;
using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.DataStructures.Graphs;
using EdlinSoftware.DataStructures.Graphs.Paths;

namespace EdlinSoftware.Algorithms.Graphs.Paths
{
    public abstract class BellmanFordShortestPathSearcherBase : ISingleSourceShortestPathSearcher
    {
        protected double[] ShortestPathValues;
        protected long?[] PreviousNodeInShortestPath;

        ISingleSourcePaths<long, double, long> ISingleSourceShortestPathSearcher.GetShortestPaths(long numberOfNodes, long initialNode, params IValuedEdge<long, double>[] edges)
        {
            return GetShortestPaths(numberOfNodes, initialNode, edges);
        }

        public abstract ISingleSourcePathsWithoutNegativeLoop<long, double, long> GetShortestPaths(long numberOfNodes,
            long initialNode, params IValuedEdge<long, double>[] edges);

        protected void InitializePathArrays(long numberOfNodes, long initialNode)
        {
            ShortestPathValues = new double[numberOfNodes];
            PreviousNodeInShortestPath = new long?[numberOfNodes];

            for (long i = 0; i < numberOfNodes; i++)
            {
                if (i == initialNode)
                    ShortestPathValues[i] = 0;
                else
                    ShortestPathValues[i] = double.PositiveInfinity;
            }
        }

        protected void RelaxEdge(IValuedEdge<long, double> edge)
        {
            double pathValueThroughEnd1 = ShortestPathValues[edge.End1] + edge.Value;
            if (pathValueThroughEnd1 < ShortestPathValues[edge.End2])
            {
                ShortestPathValues[edge.End2] = pathValueThroughEnd1;
                PreviousNodeInShortestPath[edge.End2] = edge.End1;
            }
        }

        protected bool CheckForNegativeLoop(IEnumerable<IValuedEdge<long, double>> edges)
        {
            return edges.Any(edge => ShortestPathValues[edge.End1] + edge.Value < ShortestPathValues[edge.End2]);
        }
    }

    internal class BellmanFordShortestPaths : DijkstraShortestPaths, ISingleSourcePathsWithoutNegativeLoop<long, double, long>
    {
        public bool HasNegativeLoop { get; set; }

        public BellmanFordShortestPaths(long initialNode, double[] pathValues, long?[] previousNodes)
            : base(initialNode, pathValues, previousNodes)
        { }

        public override IPath<long, double, long> GetPath(long to)
        {
            if (HasNegativeLoop) throw new InvalidOperationException();

            return base.GetPath(to);
        }
    }
}
