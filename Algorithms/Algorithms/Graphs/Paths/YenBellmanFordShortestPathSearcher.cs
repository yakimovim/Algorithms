﻿using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.DataStructures.Graphs;

namespace EdlinSoftware.Algorithms.Graphs.Paths
{
    public class YenBellmanFordShortestPathSearcher : BellmanFordShortestPathSearcherBase, ISingleSourceShortestPathSearcher
    {
        protected override int FindAllPaths()
        {
            int previousStepIndex = 0;
            int currentStepIndex = 1;

            var size = _shortestPaths.GetLongLength(0);

            for (int i = 0; i < size - 1; i++)
            {
                if (i % 2 == 0)
                {
                    for (long node = 0; node < size; node++)
                    {
                        SetPathElement(node, previousStepIndex, currentStepIndex, _inputEdges[node].Where(e => e.End1 < e.End2));
                    }
                }
                else
                {
                    for (long node = size - 1; node >= 0; node--)
                    {
                        SetPathElement(node, previousStepIndex, currentStepIndex, _inputEdges[node].Where(e => e.End1 > e.End2));
                    }
                }

                Swap(ref currentStepIndex, ref previousStepIndex);
            }

            return previousStepIndex;
        }

        private void SetPathElement(long node, int previousStepIndex, int currentStepIndex, IEnumerable<IValuedEdge<long, double>> edges)
        {
            var correctPathElement = GetCorrectedPath(node, previousStepIndex, currentStepIndex, edges);

            _shortestPaths[node, currentStepIndex] = correctPathElement.CorrectPathValue;
            _previousNodeInShortestPath[node, currentStepIndex] = correctPathElement.LastCorrectPathNode;
        }

        private CorrectPathElement GetCorrectedPath(long node, int previousSliceIndex, int currentSliceIndex, IEnumerable<IValuedEdge<long, double>> inputEdgesOfNode)
        {
            var minimumLength = _shortestPaths[node, previousSliceIndex];
            var previousNode = _previousNodeInShortestPath[node, previousSliceIndex];

            foreach (var edge in inputEdgesOfNode)
            {
                var newLength = _shortestPaths[edge.End1, currentSliceIndex] + edge.Value;

                if (minimumLength > newLength)
                {
                    minimumLength = newLength;
                    previousNode = edge.End1;
                }
            }

            return new CorrectPathElement
            {
                CorrectPathValue = minimumLength,
                LastCorrectPathNode = previousNode
            };
        }

        protected override bool CheckForNegativeCircle(int sliceIndex)
        {
            var size = _shortestPaths.GetLongLength(0);

            for (int node = 0; node < size; node++)
            {
                var correctPathElement = GetCorrectedPath(node, sliceIndex, sliceIndex, _inputEdges[node]);

                if (_shortestPaths[node, sliceIndex] > correctPathElement.CorrectPathValue)
                { return true; }
            }

            return false;
        }
    }
}
