using System;
using EdlinSoftware.Algorithms.Graphs.Paths;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Graphs.Paths
{
    [TestClass]
    public abstract class SingleSourceShortestPathTestBase<TSearcher> : ShortestPathTestBase
        where TSearcher : ISingleSourceShortestPathSearcher
    {
        protected TSearcher Searcher;

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetShortestPaths_ShouldThrowException_IfGraphHasNoNodes()
        {
            Searcher.GetShortestPaths(0, 0);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetShortestPaths_ShouldThrowException_IfSourceNodeNotInNodes()
        {
            Searcher.GetShortestPaths(10, 12);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetShortestPaths_ShouldReturnOneElementPath_IfGraphHasOneNode()
        {
            var paths = Searcher.GetShortestPaths(1, 0);

            CheckPath(paths.GetPath(0), 0.0, 0);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetShortestPaths_ShouldReturnCorrectArray_ForTwoConnectedNodes()
        {
            var paths = Searcher.GetShortestPaths(2, 0, GetDirectedValuedEdges("1 2 2"));

            CheckPath(paths.GetPath(0), 0.0, 0);
            CheckPath(paths.GetPath(1), 2.0, 0, 1);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetShortestPaths_ShouldReturnCorrectArray_ForTree()
        {
            var paths = Searcher.GetShortestPaths(6, 0, GetDirectedValuedEdges("1 3 2", "2 5 4", "1 4 3", "3 1 5", "3 2 6"));

            CheckPath(paths.GetPath(0), 0.0, 0);
            CheckPath(paths.GetPath(1), 3.0, 0, 1);
            CheckPath(paths.GetPath(2), 4.0, 0, 2);
            CheckPath(paths.GetPath(3), 8.0, 0, 1, 3);
            CheckPath(paths.GetPath(4), 5.0, 0, 2, 4);
            CheckPath(paths.GetPath(5), 6.0, 0, 2, 5);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void GetShortestPaths_ShouldReturnCorrectArray_ForComplexGraph()
        {
            var paths = Searcher.GetShortestPaths(4, 0, GetDirectedValuedEdges("1 1 2", "1 4 3", "2 2 3", "2 6 4", "3 3 4"));

            CheckPath(paths.GetPath(0), 0.0, 0);
            CheckPath(paths.GetPath(1), 1.0, 0, 1);
            CheckPath(paths.GetPath(2), 3.0, 0, 1, 2);
            CheckPath(paths.GetPath(3), 6.0, 0, 1, 2, 3);
        }
    }
}
