﻿using System.Linq;
using EdlinSoftware.Algorithms.Graphs.Paths;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Graphs.Paths
{
    [TestClass]
    public class DirectedGraphEulerianCycleSearcherTest : GraphTestBase
    {
        private DirectedGraphEulerianCycleSearcher _searcher = new DirectedGraphEulerianCycleSearcher();

        [TestInitialize]
        public void TestInitialize()
        {
            _searcher = new DirectedGraphEulerianCycleSearcher();
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetEulerianCycle_ForEmptyGraph()
        {
            Assert.IsNull(GetEulerianCycle(0));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetEulerianCycle_ForGraphWithoutEdges()
        {
            Assert.AreEqual("", GetEulerianCycle(2));
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetEulerianCycle_ForGraphWithOneEdge()
        {
            Assert.IsNull(GetEulerianCycle(2, "1-2"));
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
        public void GetEulerianCycle_SeveralEdgesBetweenTwoNodes()
        {
            Assert.AreEqual("1-2,2-1,1-2,2-1", GetEulerianCycle(2, "1-2", "1-2", "2-1", "2-1"));
        }

        private string GetEulerianCycle(int numberOfNodes, params string[] edges)
        {
            var directedGraphNodes = GetDirectedGraph(numberOfNodes, edges);

            var cycle = _searcher.GetEulerianCycle(numberOfNodes, (node) =>
            {
                var nodeEdges = directedGraphNodes[node].Edges;

                return nodeEdges.Select(e => (long)e.Second.Id - 1).ToArray();
            });

            if (cycle == null)
                return null;

            return string.Join(",", cycle.Select(p => $"{p.Item1 + 1}-{p.Item2 + 1}"));
        }
    }
}