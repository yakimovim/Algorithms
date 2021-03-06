﻿using System;
using EdlinSoftware.Algorithms.Graphs.Paths;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Graphs.Paths
{
    [TestClass]
    public abstract class BellmanFordShortestPathSearcherTestBase<TSearcher> : SingleSourceShortestPathTestBase<TSearcher>
        where TSearcher : BellmanFordShortestPathSearcherBase
    {
        [TestMethod, Owner("Ivan Yakimov")]
        public void GetShortestPaths_ShouldReturnResult_IfThereIsOnlyOneNode()
        {
            var paths = Searcher.GetShortestPaths(1, 0);

            Assert.IsFalse(paths.HasNegativeLoop);
            CheckPath(paths.GetPath(0), 0.0, 0);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetShortestPaths_ShouldReturnResult_IfThereAreTwoUnconnectedNodes()
        {
            var paths = Searcher.GetShortestPaths(2, 0);

            Assert.IsFalse(paths.HasNegativeLoop);
            CheckPath(paths.GetPath(0), 0.0, 0);
            CheckPath(paths.GetPath(1), double.PositiveInfinity);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetShortestPaths_ShouldReturnResult_IfThereIsOneBackwardEdge()
        {
            var paths = Searcher.GetShortestPaths(2, 1, GetDirectedValuedEdges("2 2 1"));

            Assert.IsFalse(paths.HasNegativeLoop);
            CheckPath(paths.GetPath(0), 2.0, 1, 0);
            CheckPath(paths.GetPath(1), 0.0, 1);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetShortestPaths_ShouldReturnResult_IfThereAreTwoConnectedNodes()
        {
            var paths = Searcher.GetShortestPaths(2, 0, GetDirectedValuedEdges("1 2 2"));

            Assert.IsFalse(paths.HasNegativeLoop);
            CheckPath(paths.GetPath(0), 0.0, 0);
            CheckPath(paths.GetPath(1), 2.0, 0, 1);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetShortestPaths_ShouldReturnResult_IfThereAreThreeConnectedNodes()
        {
            var paths = Searcher.GetShortestPaths(3, 0, GetDirectedValuedEdges("1 1 2", "2 1 3", "1 3 3"));

            Assert.IsFalse(paths.HasNegativeLoop);
            CheckPath(paths.GetPath(0), 0.0, 0);
            CheckPath(paths.GetPath(1), 1.0, 0, 1);
            CheckPath(paths.GetPath(2), 2.0, 0, 1, 2);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetShortestPaths_ShouldReturnResult_IfThereAreThreeNodesInCircleInGraph()
        {
            var paths = Searcher.GetShortestPaths(3, 0, GetDirectedValuedEdges("1 1 2", "2 2 3", "3 3 1"));

            Assert.IsFalse(paths.HasNegativeLoop);
            CheckPath(paths.GetPath(0), 0.0, 0);
            CheckPath(paths.GetPath(1), 1.0, 0, 1);
            CheckPath(paths.GetPath(2), 3.0, 0, 1, 2);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetShortestPaths_ShouldReturnResult_IfThereAreThreeNodesInCircleInGraph_WithNegativeEdges()
        {
            var paths = Searcher.GetShortestPaths(3, 0, GetDirectedValuedEdges("1 1 2", "2 -2 3", "3 3 1"));

            Assert.IsFalse(paths.HasNegativeLoop);
            CheckPath(paths.GetPath(0), 0.0, 0);
            CheckPath(paths.GetPath(1), 1.0, 0, 1);
            CheckPath(paths.GetPath(2), -1.0, 0, 1, 2);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetShortestPaths_ShouldReturnResult_IfThereIsNegativeCircleInGraph()
        {
            var paths = Searcher.GetShortestPaths(3, 0, GetDirectedValuedEdges("1 1 2", "2 -2 3", "3 -3 1"));

            Assert.IsTrue(paths.HasNegativeLoop);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetPath_ShouldThrowException_IfThereIsNegativeCircleInGraph()
        {
            var paths = Searcher.GetShortestPaths(3, 0, GetDirectedValuedEdges("1 1 2", "2 -2 3", "3 -3 1"));

            paths.GetPath(2);
        }
    }
}
