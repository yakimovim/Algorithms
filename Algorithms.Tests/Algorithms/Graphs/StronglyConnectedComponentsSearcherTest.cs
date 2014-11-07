using System;
using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.Algorithms.Graphs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Graphs
{
    [TestClass]
    public class StronglyConnectedComponentsSearcherTest
    {
        private StronglyConnectedComponentsSearcher _searcher;
        private Dictionary<long, HashSet<long>> _connectedNodes;
        private Func<long, IEnumerable<long>> _connectedNodesProvider;

        [TestInitialize]
        public void TestInitiazalie()
        {
            _searcher = new StronglyConnectedComponentsSearcher();
            _connectedNodes = new Dictionary<long, HashSet<long>>();
            _connectedNodesProvider = node => _connectedNodes.ContainsKey(node) ? (IEnumerable<long>)_connectedNodes[node] : new long[0];
        }

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Search_ShouldThrowException_IfNumberOfNodesIsNegative()
        {
            _searcher.Search(-1, _connectedNodesProvider);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_ShouldThrowException_IfConnectedNodesProviderIsNull()
        {
            _searcher.Search(10, null);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_ShouldReturnEmptyListOfComponents_IfGraphIsEmpty()
        {
            SetDirectedGraph();

            var listOfComponents = _searcher.Search(0, _connectedNodesProvider);

            Assert.AreEqual(0, listOfComponents.Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_ShouldReturnOneComponents_IfGraphHasOneNode()
        {
            SetDirectedGraph();

            var listOfComponents = _searcher.Search(1, _connectedNodesProvider);

            Assert.AreEqual(1, listOfComponents.Length);
            Assert.AreEqual(0, listOfComponents[0].First());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_ShouldReturnCorrectComponents_IfGraphHasOnlyUnconnectedNodes()
        {
            SetDirectedGraph();

            var listOfComponents = _searcher.Search(5, _connectedNodesProvider);

            CollectionAssert.AreEquivalent(new long[] { 0, 1, 2, 3, 4 }, listOfComponents.Select(c => c.First()).ToArray());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_ShouldReturnCorrectComponents_IfGraphHasOneComponent()
        {
            SetDirectedGraph("1-2", "2-3", "3-1");

            var listOfComponents = _searcher.Search(3, _connectedNodesProvider);

            CollectionAssert.AreEquivalent(new long[] { 0, 1, 2 }, listOfComponents[0].ToArray());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_ShouldReturnCorrectComponents_IfGraphHasTwoUnconnectedComponents()
        {
            SetDirectedGraph("1-2", "2-3", "3-1", "4-5", "5-6", "6-4");

            var listOfComponents = _searcher.Search(6, _connectedNodesProvider);

            CollectionAssert.AreEquivalent(new long[] { 0, 1, 2 }, listOfComponents[0].ToArray());
            CollectionAssert.AreEquivalent(new long[] { 3, 4, 5 }, listOfComponents[1].ToArray());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_ShouldReturnCorrectComponents_IfGraphHasTwoConnectedComponents()
        {
            SetDirectedGraph("1-2", "2-3", "3-1", "4-5", "5-6", "6-4", "3-5");

            var listOfComponents = _searcher.Search(6, _connectedNodesProvider);

            CollectionAssert.AreEquivalent(new long[] { 0, 1, 2 }, listOfComponents[1].ToArray());
            CollectionAssert.AreEquivalent(new long[] { 3, 4, 5 }, listOfComponents[0].ToArray());
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_ShouldReturnCorrectComponents_ForComplexGraph1()
        {
            SetDirectedGraph("1-2", "2-3", "3-1", "2-4", "3-5", "5-4", "4-7", "7-6", "6-5", "5-7", "5-10", "6-11", "10-11", "11-9", "9-10", "1-8", "8-10", "8-9");

            var listOfComponents = _searcher.Search(11, _connectedNodesProvider);

            Assert.AreEqual(4, listOfComponents.Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_ShouldReturnCorrectComponents_ForComplexGraph2()
        {
            SetDirectedGraph("1-2", "2-3", "3-1", "2-4", "4-5", "5-6", "6-4");

            var listOfComponents = _searcher.Search(6, _connectedNodesProvider);

            Assert.AreEqual(2, listOfComponents.Length);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_ShouldReturnCorrectComponents_ForComplexGraph3()
        {
            SetDirectedGraph("1-2", "2-3", "3-1", "2-4", "4-5", "5-2");

            var listOfComponents = _searcher.Search(5, _connectedNodesProvider);

            Assert.AreEqual(1, listOfComponents.Length);
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
