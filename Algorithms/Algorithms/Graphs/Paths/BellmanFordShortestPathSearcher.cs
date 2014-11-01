namespace EdlinSoftware.Algorithms.Graphs.Paths
{
    public class BellmanFordShortestPathSearcher : BellmanFordShortestPathSearcherBase, ISingleSourceShortestPathSearcher
    {
        protected override int FindAllPaths()
        {
            int previousStepIndex = 0;
            int currentStepIndex = 1;

            var size = _shortestPaths.GetLongLength(0);

            for (int i = 0; i < size - 1; i++)
            {
                for (int node = 0; node < size; node++)
                {
                    var correctPathElement = GetCorrectedPath(node, previousStepIndex);

                    _shortestPaths[node, currentStepIndex] = correctPathElement.CorrectPathValue;
                    _previousNodeInShortestPath[node, currentStepIndex] = correctPathElement.LastCorrectPathNode;
                }

                Swap(ref currentStepIndex, ref previousStepIndex);
            }

            return previousStepIndex;
        }

        private CorrectPathElement GetCorrectedPath(long node, int sliceIndex)
        {
            var minimumLength = _shortestPaths[node, sliceIndex];
            var previousNode = _previousNodeInShortestPath[node, sliceIndex];

            var inputEdgesOfNode = _inputEdges[node];

            foreach (var edge in inputEdgesOfNode)
            {
                var newLength = _shortestPaths[edge.End1, sliceIndex] + edge.Value;

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
                var correctPathElement = GetCorrectedPath(node, sliceIndex);

                if (_shortestPaths[node, sliceIndex] != correctPathElement.CorrectPathValue)
                { return true; }
            }

            return false;
        }
    }
}
