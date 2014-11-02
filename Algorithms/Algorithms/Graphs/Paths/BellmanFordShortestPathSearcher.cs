namespace EdlinSoftware.Algorithms.Graphs.Paths
{
    public class BellmanFordShortestPathSearcher : BellmanFordShortestPathSearcherBase
    {
        protected override int FindAllPaths()
        {
            int previousStepIndex = 0;
            int currentStepIndex = 1;

            var size = ShortestPaths.GetLongLength(0);

            for (int i = 0; i < size - 1; i++)
            {
                for (int node = 0; node < size; node++)
                {
                    var correctPathElement = GetCorrectedPath(node, previousStepIndex);

                    ShortestPaths[node, currentStepIndex] = correctPathElement.CorrectPathValue;
                    PreviousNodeInShortestPath[node, currentStepIndex] = correctPathElement.LastCorrectPathNode;
                }

                Swap(ref currentStepIndex, ref previousStepIndex);
            }

            return previousStepIndex;
        }

        private CorrectPathElement GetCorrectedPath(long node, int sliceIndex)
        {
            var minimumLength = ShortestPaths[node, sliceIndex];
            var previousNode = PreviousNodeInShortestPath[node, sliceIndex];

            var inputEdgesOfNode = InputEdges[node];

            foreach (var edge in inputEdgesOfNode)
            {
                var newLength = ShortestPaths[edge.End1, sliceIndex] + edge.Value;

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
                var correctPathElement = GetCorrectedPath(node, sliceIndex);

                if (ShortestPaths[node, sliceIndex] != correctPathElement.CorrectPathValue)
                { return true; }
            }

            return false;
        }
    }
}
