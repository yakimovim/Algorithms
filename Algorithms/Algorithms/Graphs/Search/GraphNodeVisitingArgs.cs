namespace EdlinSoftware.Algorithms.Graphs.Search
{
    public interface IGraphNodeVisitingArgs<TGraphNode>
    {
        TGraphNode SourceNode { get; }
        TGraphNode TargetNode { get; }
        void CancelSearch();
    }

    internal class GraphNodeVisitingArgs<TGraphNode> : IGraphNodeVisitingArgs<TGraphNode>
    {
        public TGraphNode TargetNode { get; set; }

        public TGraphNode SourceNode { get; set; }

        public bool IsSearchCancelled { get; private set; }

        public void CancelSearch()
        {
            IsSearchCancelled = true;
        }
    }
}
