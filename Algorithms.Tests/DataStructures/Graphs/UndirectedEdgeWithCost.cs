using EdlinSoftware.DataStructures.Graphs;

namespace EdlinSoftware.Tests.DataStructures.Graphs
{
    internal class UndirectedEdgeWithCost : ValuedEdge<long, long>
    {
        public UndirectedEdgeWithCost(long end1, long end2, long cost)
            : base(new UndirectedEdge<long> { End1 = end1, End2 = end2 })
        {
            Value = cost;
        }
    }
}
