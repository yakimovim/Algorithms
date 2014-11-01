using EdlinSoftware.Algorithms.Graphs.Paths;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Graphs.Paths
{
    [TestClass]
    public class YenBellmanFordShortestPathSearcherTest : BellmanFordShortestPathSearcherTestBase<YenBellmanFordShortestPathSearcher>
    {
        [TestInitialize]
        public void TestInitialize()
        {
            _searcher = new YenBellmanFordShortestPathSearcher();
        }
    }
}
