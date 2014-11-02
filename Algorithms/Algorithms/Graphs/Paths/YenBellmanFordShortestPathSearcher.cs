using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.DataStructures.Graphs;

namespace EdlinSoftware.Algorithms.Graphs.Paths
{
    public class YenBellmanFordShortestPathSearcher : BellmanFordShortestPathSearcherBase
    {
        protected override int FindAllPaths()
        {
            int previousStepIndex = 0;
            int currentStepIndex = 1;

            var size = ShortestPaths.GetLongLength(0);

            for (int i = 0; i < size - 1; i++)
            {
                if (i % 2 == 0)
                {
                    for (long node = 0; node < size; node++)
                    {
                        SetPathElement(node, previousStepIndex, currentStepIndex, InputEdges[node].Where(e => e.End1 < e.End2));
                    }
                }
                else
                {
                    for (long node = size - 1; node >= 0; node--)
                    {
                        SetPathElement(node, previousStepIndex, currentStepIndex, InputEdges[node].Where(e => e.End1 > e.End2));
                    }
                }

                Swap(ref currentStepIndex, ref previousStepIndex);
            }

            return previousStepIndex;
        }

        private void SetPathElement(long node, int previousStepIndex, int currentStepIndex, IEnumerable<IValuedEdge<long, double>> edges)
        {
            var correctPathElement = GetCorrectedPath(node, previousStepIndex, currentStepIndex, edges);

            ShortestPaths[node, currentStepIndex] = correctPathElement.CorrectPathValue;
            PreviousNodeInShortestPath[node, currentStepIndex] = correctPathElement.LastCorrectPathNode;
        }

        private CorrectPathElement GetCorrectedPath(long node, int previousSliceIndex, int currentSliceIndex, IEnumerable<IValuedEdge<long, double>> inputEdgesOfNode)
        {
            var minimumLength = ShortestPaths[node, previousSliceIndex];
            var previousNode = PreviousNodeInShortestPath[node, previousSliceIndex];

            foreach (var edge in inputEdgesOfNode)
            {
                var newLength = ShortestPaths[edge.End1, currentSliceIndex] + edge.Value;

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
            var size = ShortestPaths.GetLongLength(0);

            for (int node = 0; node < size; node++)
            {
                var correctPathElement = GetCorrectedPath(node, sliceIndex, sliceIndex, InputEdges[node]);

                if (ShortestPaths[node, sliceIndex] > correctPathElement.CorrectPathValue)
                { return true; }
            }

            return false;
        }
    }
}
