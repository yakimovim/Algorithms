using System;
using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.DataStructures.Graphs;
using EdlinSoftware.DataStructures.Graphs.Paths;

namespace EdlinSoftware.Algorithms.Graphs.Paths
{
    /// <summary>
    /// Represents searcher of shortest paths from any node to any other node without negative loops using Floyd-Warshall algorithm.
    /// </summary>
    public class FloydWarshallShortestPathSearcher : IMultiSourceShortestPathWithoutNegativeLoopSearcher
    {
        /// <summary>
        /// Returns shortest paths from any node to any other node without negative loops.
        /// </summary>
        /// <param name="numberOfNodes">Number of nodes in graph.</param>
        /// <param name="edges">Directed edges of graph.</param>
        public IMultiSourcePathsWithoutNegativeLoop<long, double, long> GetShortestPaths(long numberOfNodes, params IValuedEdge<long, double>[] edges)
        {
            var pathValueMatrix = new double[numberOfNodes, numberOfNodes, 2];
            var pathOrderMatrix = new long?[numberOfNodes, numberOfNodes];

            InitializePathMatrix(pathValueMatrix, edges);

            var sliceIndex = FindAllPaths(pathValueMatrix, pathOrderMatrix);

            var hasNegativeCircle = CheckForNegativeCircle(pathValueMatrix, sliceIndex);

            return new FloydWarshallShortestPaths(pathValueMatrix, pathOrderMatrix, sliceIndex)
            {
                HasNegativeLoop = hasNegativeCircle
            };
        }

        private void InitializePathMatrix(double[, ,] pathValueMatrix, IEnumerable<IValuedEdge<long, double>> edges)
        {
            var size = pathValueMatrix.GetLongLength(0);

            for (int from = 0; from < size; from++)
            {
                for (int to = 0; to < size; to++)
                {
                    if (from == to)
                    {
                        pathValueMatrix[from, to, 0] = 0;
                    }
                    else
                    {
                        pathValueMatrix[from, to, 0] = double.PositiveInfinity;
                    }
                }
            }

            foreach (var edge in edges)
            {
                if (edge.Value < pathValueMatrix[edge.End1, edge.End2, 0])
                {
                    pathValueMatrix[edge.End1, edge.End2, 0] = edge.Value;
                }
            }
        }

        private int FindAllPaths(double[, ,] pathValueMatrix, long?[,] pathOrderMatrix)
        {
            int previousStepIndex = 0;
            int currentStepIndex = 1;

            var size = pathValueMatrix.GetLongLength(0);

            for (long k = 0; k < size; k++)
            {
                for (long from = 0; from < size; from++)
                {
                    for (long to = 0; to < size; to++)
                    {
                        var valueOfPathWithoutKNode = pathValueMatrix[from, to, previousStepIndex];
                        var valueOfPathWithKNode = GetSum(pathValueMatrix[from, k, previousStepIndex], pathValueMatrix[k, to, previousStepIndex]);

                        pathValueMatrix[from, to, currentStepIndex] = Math.Min(valueOfPathWithoutKNode,
                            valueOfPathWithKNode);

                        if (valueOfPathWithKNode < valueOfPathWithoutKNode)
                        {
                            pathOrderMatrix[from, to] = k;
                        }
                    }
                }

                Swap(ref previousStepIndex, ref currentStepIndex);
            }

            return previousStepIndex;
        }

        private double GetSum(double p1, double p2)
        {
            if (double.IsPositiveInfinity(p1) || double.IsPositiveInfinity(p2))
                return double.PositiveInfinity;
            return p1 + p2;
        }

        private void Swap(ref int previousStepIndex, ref int currentStepIndex)
        {
            var temp = currentStepIndex;
            currentStepIndex = previousStepIndex;
            previousStepIndex = temp;
        }

        private bool CheckForNegativeCircle(double[, ,] pathValueMatrix, int sliceIndex)
        {
            var size = pathValueMatrix.GetLongLength(0);

            for (long from = 0; from < size; from++)
            {
                if (pathValueMatrix[from, from, sliceIndex] < 0)
                    return true;
            }

            return false;
        }

    }

    internal class FloydWarshallShortestPaths : IMultiSourcePathsWithoutNegativeLoop<long, double, long>
    {
        private readonly double[, ,] _pathMatrix;
        private readonly long?[,] _pathOrderMatrix;
        private readonly int _sliceIndex;

        public FloydWarshallShortestPaths(double[, ,] pathMatrix, long?[,] pathOrderMatrix, int sliceIndex)
        {
            if (pathMatrix == null) throw new ArgumentNullException(nameof(pathMatrix));
            if (pathOrderMatrix == null) throw new ArgumentNullException(nameof(pathOrderMatrix));
            _pathMatrix = pathMatrix;
            _pathOrderMatrix = pathOrderMatrix;
            _sliceIndex = sliceIndex;
        }

        public IPath<long, double, long> GetPath(long @from, long to)
        {
            if (HasNegativeLoop)
                throw new InvalidOperationException("Graph has negative loop.");

            return new ShortestPath<long, double, long>
            {
                From = @from,
                To = to,
                Value = _pathMatrix[@from, to, _sliceIndex],
                Path = GetPathOrder(@from, to).ToArray()
            };
        }

        private IEnumerable<long> GetPathOrder(long @from, long to)
        {
            if (double.IsPositiveInfinity(_pathMatrix[@from, to, _sliceIndex]))
            { return new long[0]; }

            if (_pathOrderMatrix[@from, to].HasValue == false)
            {
                return @from != to
                    ? new[] { @from, to }
                    : new[] { @from };
            }

            var middlePoint = _pathOrderMatrix[@from, to].Value;

            return GetPathOrder(@from, middlePoint).Concat(GetPathOrder(middlePoint, to).Skip(1));
        }

        public bool HasNegativeLoop { get; set; }
    }
}