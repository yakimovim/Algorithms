﻿using EdlinSoftware.Algorithms.Graphs.Paths;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Graphs.Paths
{
    [TestClass]
    public class DijkstraShortestPathSearcherTest : SingleSourceShortestPathTestBase<DijkstraShortestPathSearcher>
    {
        [TestInitialize]
        public void TestInitiazlie()
        {
            _searcher = new DijkstraShortestPathSearcher();
        }
    }
}
