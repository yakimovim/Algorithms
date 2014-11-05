using EdlinSoftware.Algorithms.Graphs.Paths;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Graphs.Paths
{
    [TestClass]
    public class FloydWarshallShortestPathSearcherTest : MultiSourceShortestPathSearcherTest<FloydWarshallShortestPathSearcher>
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Searcher = new FloydWarshallShortestPathSearcher();
        }
    }
}