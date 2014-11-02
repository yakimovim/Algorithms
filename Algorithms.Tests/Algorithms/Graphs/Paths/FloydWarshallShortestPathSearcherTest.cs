using System;
using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.Algorithms.Graphs.Paths;
using EdlinSoftware.DataStructures.Graphs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Graphs.Paths
{
    [TestClass]
    public class FloydWarshallShortestPathSearcherTest
    {
        private FloydWarshallShortestPathSearcher _searcher;

        [TestInitialize]
        public void TestInitialize()
        {
            _searcher = new FloydWarshallShortestPathSearcher();
        }

        [TestMethod]
        public void Search_ShouldReturnResult_IfThereAreNoNodesInGraph()
        {
            var allPaths = _searcher.GetShortestPaths(0);

            Assert.IsFalse(allPaths.HasNegativeLoop);
        }

        [TestMethod]
        public void Search_ShouldReturnResult_IfThereIsOneNodeInGraph()
        {
            var allPaths = _searcher.GetShortestPaths(1);

            Assert.IsFalse(allPaths.HasNegativeLoop);
            Assert.AreEqual(0, allPaths.GetPath(0, 0).Value);
        }

        [TestMethod]
        public void Search_ShouldReturnResult_IfThereAreTwoConnectedNodesInGraph()
        {
            var allPaths = _searcher.GetShortestPaths(2, GetEdges("0 1 2"));

            Assert.IsFalse(allPaths.HasNegativeLoop);
            Assert.AreEqual(0, allPaths.GetPath(0, 0).Value);
            CollectionAssert.AreEqual(new long[] { 0 }, allPaths.GetPath(0, 0).Path.ToArray());
            Assert.AreEqual(0, allPaths.GetPath(1, 1).Value);
            CollectionAssert.AreEqual(new long[] { 1 }, allPaths.GetPath(1, 1).Path.ToArray());
            Assert.AreEqual(2, allPaths.GetPath(0, 1).Value);
            CollectionAssert.AreEqual(new long[] { 0, 1 }, allPaths.GetPath(0, 1).Path.ToArray());
            Assert.AreEqual(double.PositiveInfinity, allPaths.GetPath(1, 0).Value);
        }

        [TestMethod]
        public void Search_ShouldReturnResult_IfThereAreTwoUnconnectedNodesInGraph()
        {
            var allPaths = _searcher.GetShortestPaths(2);

            Assert.IsFalse(allPaths.HasNegativeLoop);
            Assert.AreEqual(0, allPaths.GetPath(0, 0).Value);
            CollectionAssert.AreEqual(new long[] { 0 }, allPaths.GetPath(0, 0).Path.ToArray());
            Assert.AreEqual(0, allPaths.GetPath(1, 1).Value);
            CollectionAssert.AreEqual(new long[] { 1 }, allPaths.GetPath(1, 1).Path.ToArray());
            Assert.AreEqual(double.PositiveInfinity, allPaths.GetPath(0, 1).Value);
            Assert.IsFalse(allPaths.GetPath(0, 1).Path.Any());
            Assert.AreEqual(double.PositiveInfinity, allPaths.GetPath(1, 0).Value);
            Assert.IsFalse(allPaths.GetPath(1, 0).Path.Any());
        }

        [TestMethod]
        public void Search_ShouldReturnResult_IfThereAreThreeNodesInGraph()
        {
            var allPaths = _searcher.GetShortestPaths(3, GetEdges("0 1 1, 1 2 1, 0 2 3"));

            Assert.IsFalse(allPaths.HasNegativeLoop);
            Assert.AreEqual(0, allPaths.GetPath(0, 0).Value);
            CollectionAssert.AreEqual(new long[] { 0 }, allPaths.GetPath(0, 0).Path.ToArray());
            Assert.AreEqual(0, allPaths.GetPath(1, 1).Value);
            CollectionAssert.AreEqual(new long[] { 1 }, allPaths.GetPath(1, 1).Path.ToArray());
            Assert.AreEqual(0, allPaths.GetPath(2, 2).Value);
            CollectionAssert.AreEqual(new long[] { 2 }, allPaths.GetPath(2, 2).Path.ToArray());
            Assert.AreEqual(1, allPaths.GetPath(0, 1).Value);
            CollectionAssert.AreEqual(new long[] { 0, 1 }, allPaths.GetPath(0, 1).Path.ToArray());
            Assert.AreEqual(2, allPaths.GetPath(0, 2).Value);
            CollectionAssert.AreEqual(new long[] { 0, 1, 2 }, allPaths.GetPath(0, 2).Path.ToArray());
            Assert.AreEqual(double.PositiveInfinity, allPaths.GetPath(1, 0).Value);
            Assert.IsFalse(allPaths.GetPath(1, 0).Path.Any());
            Assert.AreEqual(1, allPaths.GetPath(1, 2).Value);
            CollectionAssert.AreEqual(new long[] { 1, 2 }, allPaths.GetPath(1, 2).Path.ToArray());
            Assert.AreEqual(double.PositiveInfinity, allPaths.GetPath(2, 0).Value);
            Assert.IsFalse(allPaths.GetPath(2, 0).Path.Any());
            Assert.AreEqual(double.PositiveInfinity, allPaths.GetPath(2, 1).Value);
            Assert.IsFalse(allPaths.GetPath(2, 1).Path.Any());
        }

        [TestMethod]
        public void Search_ShouldReturnResult_IfThereAreThreeNodesInCircleInGraph()
        {
            var allPaths = _searcher.GetShortestPaths(3, GetEdges("0 1 1, 1 2 2, 2 0 3"));

            Assert.IsFalse(allPaths.HasNegativeLoop);
            Assert.AreEqual(0, allPaths.GetPath(0, 0).Value);
            CollectionAssert.AreEqual(new long[] { 0 }, allPaths.GetPath(0, 0).Path.ToArray());
            Assert.AreEqual(0, allPaths.GetPath(1, 1).Value);
            CollectionAssert.AreEqual(new long[] { 1 }, allPaths.GetPath(1, 1).Path.ToArray());
            Assert.AreEqual(0, allPaths.GetPath(2, 2).Value);
            CollectionAssert.AreEqual(new long[] { 2 }, allPaths.GetPath(2, 2).Path.ToArray());
            Assert.AreEqual(1, allPaths.GetPath(0, 1).Value);
            CollectionAssert.AreEqual(new long[] { 0, 1 }, allPaths.GetPath(0, 1).Path.ToArray());
            Assert.AreEqual(3, allPaths.GetPath(0, 2).Value);
            CollectionAssert.AreEqual(new long[] { 0, 1, 2 }, allPaths.GetPath(0, 2).Path.ToArray());
            Assert.AreEqual(5, allPaths.GetPath(1, 0).Value);
            CollectionAssert.AreEqual(new long[] { 1, 2, 0 }, allPaths.GetPath(1, 0).Path.ToArray());
            Assert.AreEqual(2, allPaths.GetPath(1, 2).Value);
            CollectionAssert.AreEqual(new long[] { 1, 2 }, allPaths.GetPath(1, 2).Path.ToArray());
            Assert.AreEqual(3, allPaths.GetPath(2, 0).Value);
            CollectionAssert.AreEqual(new long[] { 2, 0 }, allPaths.GetPath(2, 0).Path.ToArray());
            Assert.AreEqual(4, allPaths.GetPath(2, 1).Value);
            CollectionAssert.AreEqual(new long[] { 2, 0, 1 }, allPaths.GetPath(2, 1).Path.ToArray());
        }

        [TestMethod]
        public void Search_ShouldReturnResult_IfThereAreThreeNodesInCircleInGraph_WithNegativeEdges()
        {
            var allPaths = _searcher.GetShortestPaths(3, GetEdges("0 1 1, 1 2 -2, 2 0 3"));

            Assert.IsFalse(allPaths.HasNegativeLoop);
            Assert.AreEqual(0, allPaths.GetPath(0, 0).Value);
            CollectionAssert.AreEqual(new long[] { 0 }, allPaths.GetPath(0, 0).Path.ToArray());
            Assert.AreEqual(0, allPaths.GetPath(1, 1).Value);
            CollectionAssert.AreEqual(new long[] { 1 }, allPaths.GetPath(1, 1).Path.ToArray());
            Assert.AreEqual(0, allPaths.GetPath(2, 2).Value);
            CollectionAssert.AreEqual(new long[] { 2 }, allPaths.GetPath(2, 2).Path.ToArray());
            Assert.AreEqual(1, allPaths.GetPath(0, 1).Value);
            CollectionAssert.AreEqual(new long[] { 0, 1 }, allPaths.GetPath(0, 1).Path.ToArray());
            Assert.AreEqual(-1, allPaths.GetPath(0, 2).Value);
            CollectionAssert.AreEqual(new long[] { 0, 1, 2 }, allPaths.GetPath(0, 2).Path.ToArray());
            Assert.AreEqual(1, allPaths.GetPath(1, 0).Value);
            CollectionAssert.AreEqual(new long[] { 1, 2, 0 }, allPaths.GetPath(1, 0).Path.ToArray());
            Assert.AreEqual(-2, allPaths.GetPath(1, 2).Value);
            CollectionAssert.AreEqual(new long[] { 1, 2 }, allPaths.GetPath(1, 2).Path.ToArray());
            Assert.AreEqual(3, allPaths.GetPath(2, 0).Value);
            CollectionAssert.AreEqual(new long[] { 2, 0 }, allPaths.GetPath(2, 0).Path.ToArray());
            Assert.AreEqual(4, allPaths.GetPath(2, 1).Value);
            CollectionAssert.AreEqual(new long[] { 2, 0, 1 }, allPaths.GetPath(2, 1).Path.ToArray());
        }

        [TestMethod]
        public void Search_ShouldReturnResult_IfThereIsNegativeCircleInGraph()
        {
            var allPaths = _searcher.GetShortestPaths(3, GetEdges("0 1 1, 1 2 -2, 2 0 -3"));

            Assert.IsTrue(allPaths.HasNegativeLoop);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetPathLength_ShouldThrowException_IfThereIsNegativeCircleInGraph()
        {
            var allPaths = _searcher.GetShortestPaths(3, GetEdges("0 1 1, 1 2 -2, 2 0 -3"));

            allPaths.GetPath(2, 1);
        }

        private IValuedEdge<long, double>[] GetEdges(string description)
        {
            var edgesDescriptions = description.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            var edges = new List<IValuedEdge<long, double>>();

            foreach (var edgeDescription in edgesDescriptions)
            {
                var parts = edgeDescription.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                edges.Add(ValuedEdge<long, double>.Directed(EqualityComparer<long>.Default, edge =>
                {
                    edge.End1 = long.Parse(parts[0]);
                    edge.End2 = long.Parse(parts[1]);
                    edge.Value = long.Parse(parts[2]);
                }));
            }

            return edges.ToArray();
        }
    }
}