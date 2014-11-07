using EdlinSoftware.Algorithms.Graphs.MinimumSpanningTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Graphs.MinimumSpanningTree
{
    [TestClass]
    public class PrimAlgorithmTest : MinimumSpanningTreeAlgorithmTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            MstSearcher = new PrimAlgorithm();
        }
    }
}
