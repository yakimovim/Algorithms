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

    internal class BellmanFordShortestPaths : ISingleSourcePathsWithoutNegativeLoop<long, double, long>
    {
        private readonly double[] _pathMatrix;
        private readonly long?[] _previousNodes;
        private readonly long _initialNode;

        public bool HasNegativeLoop { get; set; }

        public BellmanFordShortestPaths(long initialNode, double[] pathMatrix, long?[] previousNodes)
        {
            if (pathMatrix == null) throw new ArgumentNullException("pathMatrix");
            if (previousNodes == null) throw new ArgumentNullException("previousNodes");
            _pathMatrix = pathMatrix;
            _previousNodes = previousNodes;
            _initialNode = initialNode;
        }

        public IPath<long, double, long> GetPath(long to)
        {
            if (HasNegativeLoop) throw new InvalidOperationException();

            return new ShortestPath<long, double, long>
            {
                From = _initialNode,
                To = to,
                Value = _pathMatrix[to],
                Path = GetPathTo(to)
            };
        }

        private IEnumerable<long> GetPathTo(long to)
        {
            if (to == _initialNode)
                return new[] { _initialNode };
            if (_previousNodes[to].HasValue == false)
                return new long[0];

            var path = new List<long> { to };

            while (true)
            {
                if (_previousNodes[to].HasValue == false)
                { throw new InvalidOperationException("Incorrect path sequence."); }
                var previousNode = _previousNodes[to].Value;

                if (previousNode == _initialNode)
                { break; }

                path.Insert(0, previousNode);
                to = previousNode;
            }

            path.Insert(0, _initialNode);

            return path;
        }
    }
}
