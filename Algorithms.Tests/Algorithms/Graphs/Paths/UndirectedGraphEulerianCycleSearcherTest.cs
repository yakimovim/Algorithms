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

        private string GetEulerianCycle(int numberOfNodes, params string[] edges)
        {
            var directedGraphNodes = GetUndirectedGraph(numberOfNodes, edges);

            var cycle = _searcher.GetEulerianCycle(numberOfNodes, (node) =>
            {
                var nodeEdges = directedGraphNodes[node].Edges;

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