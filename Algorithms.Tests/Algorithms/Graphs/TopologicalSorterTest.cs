using System;
using EdlinSoftware.Algorithms.Graphs;
using EdlinSoftware.Tests.DataStructures.Graphs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Graphs
{
    [TestClass]
    public class TopologicalSorterTest : GraphTestBase
    {
        private TopologicalSorter<GraphNode> _sorter;

        [TestInitialize]
        public void TestInitialize()
        {
            _sorter = new TopologicalSorter<GraphNode>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Sort_ShouldThrowException_IfGraphIsNull()
        {
            _sorter.Sort(null, _connectedNodesSelector);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Sort_ShouldThrowException_IfNodesSelectorIsNull()
        {
            _sorter.Sort(GetDirectedGraph(1), null);
        }

        [TestMethod]
        public void Sort_ShouldReturnEmptyArray_IfGraphIsEmpty()
        {
            var order = _sorter.Sort(GetDirectedGraph(0), _connectedNodesSelector);

            Assert.AreEqual(0, order.Length);
        }

        [TestMethod]
        public void Sort_ShouldReturnArrayWithOneElement_IfGraphHasOneNode()
        {
            var graph = GetDirectedGraph(1);
            var order = _sorter.Sort(graph, _connectedNodesSelector);

            Assert.AreEqual(1, order.Length);
            Assert.AreSame(graph[0], order[0]);
        }

        [TestMethod]
        public void Sort_ShouldReturnCorrectOrder_ForChain()
        {
            var graph = GetDirectedGraph(3, "1-2", "2-3");
            var order = _sorter.Sort(graph, _connectedNodesSelector);

            CollectionAssert.AreEqual(new[] { graph[0], graph[1], graph[2] }, order);
        }

        [TestMethod]
        public void Sort_ShouldReturnCorrectOrder_ForInversedChain()
        {
            var graph = GetDirectedGraph(3, "2-1", "3-2");
            var order = _sorter.Sort(graph, _connectedNodesSelector);

            CollectionAssert.AreEqual(new[] { graph[2], graph[1], graph[0] }, order);
        }

        [TestMethod]
        public void Sort_ShouldReturnCorrectOrder_ForComplexConnections()
        {
            var graph = GetDirectedGraph(4, "1-2", "1-3", "4-2", "4-3");
            var order = _sorter.Sort(graph, _connectedNodesSelector);

            CollectionAssert.AreEqual(new[] { graph[3], graph[0], graph[2], graph[1] }, order);
        }

        [TestMethod]
        public void Sort_ShouldReturnCorrectOrder_ForGraphWithSeveralComponents()
        {
            var graph = GetDirectedGraph(6, "1-2", "1-3", "4-2", "4-3", "5-6");
            var order = _sorter.Sort(graph, _connectedNodesSelector);

            CollectionAssert.AreEqual(new[] { graph[4], graph[5], graph[3], graph[0], graph[2], graph[1] }, order);
        }
    }
}
