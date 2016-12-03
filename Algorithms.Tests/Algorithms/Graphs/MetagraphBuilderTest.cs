using System;
using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.Algorithms.Graphs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Graphs
{
    [TestClass]
    public class MetagraphBuilderTest
    {
        private MetagraphBuilder _builder;
        private Dictionary<long, HashSet<long>> _connectedNodes;
        private Func<long, IEnumerable<long>> _connectedNodesProvider;

        [TestInitialize]
        public void TestInitiazalie()
        {
            _builder = new MetagraphBuilder();
            _connectedNodes = new Dictionary<long, HashSet<long>>();
            _connectedNodesProvider = node => _connectedNodes.ContainsKey(node) ? (IEnumerable<long>)_connectedNodes[node] : new long[0];
        }

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void BuildMetagraph_ShouldThrowException_IfNumberOfNodesIsNegative()
        {
            _builder.BuildMetagraph(-1, _connectedNodesProvider);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BuildMetagraph_ShouldThrowException_IfConnectedNodesProviderIsNull()
        {
            _builder.BuildMetagraph(10, null);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void BuildMetagraph_ShouldReturnEmptyMetagraph_IfGraphIsEmpty()
        {
            SetDirectedGraph();

            var metagraph = _builder.BuildMetagraph(0, _connectedNodesProvider);

            Assert.IsNotNull(metagraph);
            Assert.AreEqual(0, metagraph.NumberOfNodes);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void BuildMetagraph_ShouldReturnMetagraphWithOneNode_IfGraphHasOneNode()
        {
            SetDirectedGraph();

            var metagraph = _builder.BuildMetagraph(1, _connectedNodesProvider);

            Assert.IsNotNull(metagraph);
            Assert.AreEqual(1, metagraph.NumberOfNodes);
            Assert.AreEqual(0, metagraph.EdgesProvider(0).Count());
            CollectionAssert.AreEquivalent(new[] { 0L }, metagraph.ComponentNodes(0).ToArray());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void BuildMetagraph_ShouldReturnCorrectMetagraph_IfGraphHasOnlyUnconnectedNodes()
        {
            SetDirectedGraph();

            var metagraph = _builder.BuildMetagraph(5, _connectedNodesProvider);

            Assert.IsNotNull(metagraph);
            Assert.AreEqual(5, metagraph.NumberOfNodes);

            for (int i = 0; i < 5; i++)
            {
                Assert.AreEqual(0, metagraph.EdgesProvider(i).Count());
                CollectionAssert.AreEquivalent(new[] { (long)i }, metagraph.ComponentNodes(i).ToArray());
            }
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void BuildMetagraph_ShouldReturnCorrectMetagraph_IfGraphHasOneComponent()
        {
            SetDirectedGraph("1-2", "2-3", "3-1");

            var metagraph = _builder.BuildMetagraph(3, _connectedNodesProvider);

            Assert.IsNotNull(metagraph);
            Assert.AreEqual(1, metagraph.NumberOfNodes);
            Assert.AreEqual(0, metagraph.EdgesProvider(0).Count());
            CollectionAssert.AreEquivalent(new[] { 0L, 1L, 2L }, metagraph.ComponentNodes(0).ToArray());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void BuildMetagraph_ShouldReturnCorrectMetagraph_IfGraphHasTwoUnconnectedComponents()
        {
            SetDirectedGraph("1-2", "2-3", "3-1", "4-5", "5-6", "6-4");

            var metagraph = _builder.BuildMetagraph(6, _connectedNodesProvider);

            Assert.IsNotNull(metagraph);
            Assert.AreEqual(2, metagraph.NumberOfNodes);
            Assert.AreEqual(0, metagraph.EdgesProvider(0).Count());
            Assert.AreEqual(0, metagraph.EdgesProvider(1).Count());
            CollectionAssert.AreEquivalent(new long[] { 0, 1, 2 }, metagraph.ComponentNodes(0).ToArray());
            CollectionAssert.AreEquivalent(new long[] { 3, 4, 5 }, metagraph.ComponentNodes(1).ToArray());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void BuildMetagraph_ShouldReturnCorrectMetagraph_IfGraphHasTwoConnectedComponents()
        {
            SetDirectedGraph("1-2", "2-3", "3-1", "4-5", "5-6", "6-4", "3-5");

            var metagraph = _builder.BuildMetagraph(6, _connectedNodesProvider);

            Assert.IsNotNull(metagraph);
            Assert.AreEqual(2, metagraph.NumberOfNodes);
            Assert.AreEqual(0, metagraph.EdgesProvider(0).Count());
            CollectionAssert.AreEquivalent(new[] { 0L }, metagraph.EdgesProvider(1).ToArray());
            CollectionAssert.AreEquivalent(new long[] { 3, 4, 5 }, metagraph.ComponentNodes(0).ToArray());
            CollectionAssert.AreEquivalent(new long[] { 0, 1, 2 }, metagraph.ComponentNodes(1).ToArray());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void BuildMetagraph_ShouldReturnCorrectMetagraph_ForComplexGraph1()
        {
            SetDirectedGraph("1-2", "2-3", "3-1", "2-4", "3-5", "5-4", "4-7", "7-6", "6-5", "5-7", "5-10", "6-11", "10-11", "11-9", "9-10", "1-8", "8-10", "8-9");

            var metagraph = _builder.BuildMetagraph(11, _connectedNodesProvider);

            Assert.IsNotNull(metagraph);
            Assert.AreEqual(4, metagraph.NumberOfNodes);
            Assert.AreEqual(0, metagraph.EdgesProvider(0).Count());
            CollectionAssert.AreEquivalent(new[] { 0L }, metagraph.EdgesProvider(1).ToArray());
            CollectionAssert.AreEquivalent(new[] { 0L }, metagraph.EdgesProvider(2).ToArray());
            CollectionAssert.AreEquivalent(new[] { 1L, 2L }, metagraph.EdgesProvider(3).ToArray());
            CollectionAssert.AreEquivalent(new long[] { 8, 9, 10 }, metagraph.ComponentNodes(0).ToArray());
            CollectionAssert.AreEquivalent(new long[] { 7 }, metagraph.ComponentNodes(1).ToArray());
            CollectionAssert.AreEquivalent(new long[] { 3, 4, 5, 6 }, metagraph.ComponentNodes(2).ToArray());
            CollectionAssert.AreEquivalent(new long[] { 0, 1, 2 }, metagraph.ComponentNodes(3).ToArray());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void BuildMetagraph_ShouldReturnCorrectMetagraph_ForComplexGraph2()
        {
            SetDirectedGraph("1-2", "2-3", "3-1", "2-4", "4-5", "5-6", "6-4");

            var metagraph = _builder.BuildMetagraph(6, _connectedNodesProvider);

            Assert.IsNotNull(metagraph);
            Assert.AreEqual(2, metagraph.NumberOfNodes);
            Assert.AreEqual(0, metagraph.EdgesProvider(0).Count());
            CollectionAssert.AreEquivalent(new[] { 0L }, metagraph.EdgesProvider(1).ToArray());
            CollectionAssert.AreEquivalent(new long[] { 3, 4, 5 }, metagraph.ComponentNodes(0).ToArray());
            CollectionAssert.AreEquivalent(new long[] { 0, 1, 2 }, metagraph.ComponentNodes(1).ToArray());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void BuildMetagraph_ShouldReturnCorrectMetagraph_ForComplexGraph3()
        {
            SetDirectedGraph("1-2", "2-3", "3-1", "2-4", "4-5", "5-2");

            var metagraph = _builder.BuildMetagraph(5, _connectedNodesProvider);

            Assert.IsNotNull(metagraph);
            Assert.AreEqual(1, metagraph.NumberOfNodes);
            Assert.AreEqual(0, metagraph.EdgesProvider(0).Count());
            CollectionAssert.AreEquivalent(new long[] { 0, 1, 2, 3, 4 }, metagraph.ComponentNodes(0).ToArray());
        }

        private void SetDirectedGraph(params string[] edges)
        {
            foreach (var edge in edges)
            {
                var parts = edge.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

                var fromNode = int.Parse(parts[0]) - 1;
                var toNode = int.Parse(parts[1]) - 1;

                if (_connectedNodes.ContainsKey(fromNode) == false)
                    _connectedNodes[fromNode] = new HashSet<long>();
                _connectedNodes[fromNode].Add(toNode);
            }
        }
    }
}