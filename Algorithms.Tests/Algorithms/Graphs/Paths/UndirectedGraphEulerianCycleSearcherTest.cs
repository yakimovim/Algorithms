using EdlinSoftware.Algorithms.Graphs.Paths;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace EdlinSoftware.Tests.Algorithms.Graphs.Paths
{
    [TestClass]
    public class UndirectedGraphEulerianCycleSearcherTest : GraphTestBase
    {
        private UndirectedGraphEulerianCycleSearcher _searcher;

        [TestInitialize]
        public void TestInitialize()
        {
            _searcher = new UndirectedGraphEulerianCycleSearcher();
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetEulerianCycle_ForEmptyGraph()
        {
            var eulerianCycle = GetEulerianCycle(0);

            Assert.IsNull(eulerianCycle);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetEulerianCycle_ForGraphWithOneEdge()
        {
            var eulerianCycle = GetEulerianCycle(2, "1-2");

            Assert.IsNull(eulerianCycle);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetEulerianCycle_ForGraphWithTwoEdgesBetweenTwoNodes()
        {
            var eulerianCycle = GetEulerianCycle(2, "1-2", "1-2");

            Assert.IsNotNull(eulerianCycle);
            Assert.AreEqual("1-2,2-1", eulerianCycle);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetEulerianCycle_ForGraphWithSimpleCycle()
        {
            Assert.AreEqual("1-2,2-1", GetEulerianCycle(2, "1-2", "2-1"));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetEulerianCycle_ForGraphWithOneNodeInSeveralCycles()
        {
            Assert.AreEqual("1-2,2-3,3-2,2-1", GetEulerianCycle(3, "1-2", "2-3", "3-2", "2-1"));
            Assert.AreEqual("1-2,2-3,3-2,2-1", GetEulerianCycle(3, "1-2", "2-1", "2-3", "3-2"));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetEulerianCycle_FourPetalFlower()
        {
            Assert.AreEqual("1-2,2-5,5-2,2-4,4-2,2-3,3-2,2-1", GetEulerianCycle(5, "2-1", "1-2", "2-3", "3-2", "2-4", "4-2", "2-5", "5-2"));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetEulerianCycle_Different()
        {
            Assert.AreEqual("1-2,2-2,2-3,3-1", GetEulerianCycle(3, "2-3", "2-2", "1-2", "3-1"));
            Assert.IsNull(GetEulerianCycle(3, "1-3", "2-3", "1-2", "3-1"));
            Assert.AreEqual("1-4,4-2,2-3,3-4,4-1,1-2,2-1", GetEulerianCycle(4, "1-2", "2-1", "1-4", "4-1", "2-4", "3-2", "4-3"));
            Assert.AreEqual("1-3,3-2,2-4,4-2,2-3,3-4,4-1", GetEulerianCycle(4, "2-3", "3-4", "1-4", "3-1", "4-2", "2-3", "4-2"));
        }

        private string GetEulerianCycle(int numberOfNodes, params string[] edges)
        {
            var undirectedGraphNodes = GetUndirectedGraph(numberOfNodes, edges);

            var cycle = _searcher.GetEulerianCycle(numberOfNodes, (node) =>
            {
                var nodeEdges = undirectedGraphNodes[node].Edges;

                return nodeEdges.Select(e =>
                {
                    var end1 = (long)e.First.Id - 1;
                    var end2 = (long)e.Second.Id - 1;

                    return node == end1 ? end2 : end1;
                }).ToArray();
            });

            if (cycle == null)
                return null;

            return string.Join(",", cycle.Select(p => $"{p.Item1 + 1}-{p.Item2 + 1}"));
        }
    }
}