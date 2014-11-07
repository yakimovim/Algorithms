using EdlinSoftware.Algorithms.Graphs.MinimumSpanningTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Graphs.MinimumSpanningTree
{
    [TestClass]
    public class KruskalAlgorithmTest : MinimumSpanningTreeAlgorithmTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            MstSearcher = new KruskalAlgorithm();
        }
    }
}
