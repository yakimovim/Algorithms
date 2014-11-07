using System.Collections.Generic;
using EdlinSoftware.Algorithms.Graphs.Search;
using EdlinSoftware.Tests.DataStructures.Graphs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Graphs.Search
{
    [TestClass]
    public abstract class GraphSearcherTestBase<TGraphSearcher> : GraphTestBase
        where TGraphSearcher : IGraphSearcher<GraphNode>
    {
        protected TGraphSearcher Searcher;
        protected List<GraphNode> VisitedNodes;
    }
}
