using System;
using EdlinSoftware.Algorithms.Graphs.Paths;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Graphs.Paths
{
    [TestClass]
    public abstract class MultiSourceShortestPathSearcherTest<TSearcher> : ShortestPathTestBase
        where TSearcher : IMultiSourceShortestPathWithoutNegativeLoopSearcher
    {
        protected TSearcher Searcher;

        [TestMethod]
        public void Search_ShouldReturnResult_IfThereAreNoNodesInGraph()
        {
            var allPaths = Searcher.GetShortestPaths(0);

            Assert.IsFalse(allPaths.HasNegativeLoop);
        }

        [TestMethod]
        public void Search_ShouldReturnResult_IfThereIsOneNodeInGraph()
        {
            var allPaths = Searcher.GetShortestPaths(1);

            Assert.IsFalse(allPaths.HasNegativeLoop);
            CheckPath(allPaths.GetPath(0, 0), 0.0, new long[] { 0 });
        }

        [TestMethod]
        public void Search_ShouldReturnResult_IfThereAreTwoConnectedNodesInGraph()
        {
            var allPaths = Searcher.GetShortestPaths(2, GetDirectedValuedEdges("1 2 2"));

            Assert.IsFalse(allPaths.HasNegativeLoop);
            CheckPath(allPaths.GetPath(0, 0), 0.0, new long[] { 0 });
            CheckPath(allPaths.GetPath(1, 1), 0.0, new long[] { 1 });
            CheckPath(allPaths.GetPath(0, 1), 2.0, new long[] { 0, 1 });
            CheckPath(allPaths.GetPath(1, 0), double.PositiveInfinity, new long[0]);
        }

        [TestMethod]
        public void Search_ShouldReturnResult_IfThereAreTwoUnconnectedNodesInGraph()
        {
            var allPaths = Searcher.GetShortestPaths(2);

            Assert.IsFalse(allPaths.HasNegativeLoop);
            CheckPath(allPaths.GetPath(0, 0), 0.0, new long[] { 0 });
            CheckPath(allPaths.GetPath(1, 1), 0.0, new long[] { 1 });
            CheckPath(allPaths.GetPath(0, 1), double.PositiveInfinity, new long[0]);
            CheckPath(allPaths.GetPath(1, 0), double.PositiveInfinity, new long[0]);
        }

        [TestMethod]
        public void Search_ShouldReturnResult_IfThereAreThreeNodesInGraph()
        {
            var allPaths = Searcher.GetShortestPaths(3, GetDirectedValuedEdges("1 1 2", "2 1 3", "1 3 3"));

            Assert.IsFalse(allPaths.HasNegativeLoop);
            CheckPath(allPaths.GetPath(0, 0), 0.0, new long[] { 0 });
            CheckPath(allPaths.GetPath(1, 1), 0.0, new long[] { 1 });
            CheckPath(allPaths.GetPath(2, 2), 0.0, new long[] { 2 });
            CheckPath(allPaths.GetPath(0, 1), 1.0, new long[] { 0, 1 });
            CheckPath(allPaths.GetPath(0, 2), 2.0, new long[] { 0, 1, 2 });
            CheckPath(allPaths.GetPath(1, 0), double.PositiveInfinity, new long[0]);
            CheckPath(allPaths.GetPath(1, 2), 1.0, new long[] { 1, 2 });
            CheckPath(allPaths.GetPath(2, 0), double.PositiveInfinity, new long[0]);
            CheckPath(allPaths.GetPath(2, 1), double.PositiveInfinity, new long[0]);
        }

        [TestMethod]
        public void Search_ShouldReturnResult_IfThereAreThreeNodesInCircleInGraph()
        {
            var allPaths = Searcher.GetShortestPaths(3, GetDirectedValuedEdges("1 1 2", "2 2 3", "3 3 1"));

            Assert.IsFalse(allPaths.HasNegativeLoop);
            CheckPath(allPaths.GetPath(0, 0), 0.0, new long[] { 0 });
            CheckPath(allPaths.GetPath(1, 1), 0.0, new long[] { 1 });
            CheckPath(allPaths.GetPath(2, 2), 0.0, new long[] { 2 });
            CheckPath(allPaths.GetPath(0, 1), 1.0, new long[] { 0, 1 });
            CheckPath(allPaths.GetPath(0, 2), 3.0, new long[] { 0, 1, 2 });
            CheckPath(allPaths.GetPath(1, 0), 5.0, new long[] { 1, 2, 0 });
            CheckPath(allPaths.GetPath(1, 2), 2.0, new long[] { 1, 2 });
            CheckPath(allPaths.GetPath(2, 0), 3.0, new long[] { 2, 0 });
            CheckPath(allPaths.GetPath(2, 1), 4.0, new long[] { 2, 0, 1 });
        }

        [TestMethod]
        public void Search_ShouldReturnResult_IfThereAreThreeNodesInCircleInGraph_WithNegativeEdges()
        {
            var allPaths = Searcher.GetShortestPaths(3, GetDirectedValuedEdges("1 1 2", "2 -2 3", "3 3 1"));

            Assert.IsFalse(allPaths.HasNegativeLoop);
            CheckPath(allPaths.GetPath(0, 0), 0.0, new long[] { 0 });
            CheckPath(allPaths.GetPath(1, 1), 0.0, new long[] { 1 });
            CheckPath(allPaths.GetPath(2, 2), 0.0, new long[] { 2 });
            CheckPath(allPaths.GetPath(0, 1), 1.0, new long[] { 0, 1 });
            CheckPath(allPaths.GetPath(0, 2), -1.0, new long[] { 0, 1, 2 });
            CheckPath(allPaths.GetPath(1, 0), 1.0, new long[] { 1, 2, 0 });
            CheckPath(allPaths.GetPath(1, 2), -2.0, new long[] { 1, 2 });
            CheckPath(allPaths.GetPath(2, 0), 3.0, new long[] { 2, 0 });
            CheckPath(allPaths.GetPath(2, 1), 4.0, new long[] { 2, 0, 1 });
        }

        [TestMethod]
        public void Search_ShouldReturnResult_IfThereIsNegativeCircleInGraph()
        {
            var allPaths = Searcher.GetShortestPaths(3, GetDirectedValuedEdges("1 1 2", "2 -2 3", "3 -3 1"));

            Assert.IsTrue(allPaths.HasNegativeLoop);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetPathLength_ShouldThrowException_IfThereIsNegativeCircleInGraph()
        {
            var allPaths = Searcher.GetShortestPaths(3, GetDirectedValuedEdges("1 1 2", "2 -2 3", "3 -3 1"));

            allPaths.GetPath(2, 1);
        }
    }
}