using System.Linq;
using EdlinSoftware.Algorithms.Graphs.MinimumSpanningTree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Edge = EdlinSoftware.Tests.DataStructures.Graphs.UndirectedEdgeWithCost;

namespace EdlinSoftware.Tests.Algorithms.Graphs.MinimumSpanningTree
{
    [TestClass]
    public abstract class MinimumSpanningTreeAlgorithmTest
    {
        protected IMinimumSpanningTreeAlgorithm _mstSearcher;

        [TestMethod]
        public void GetMinimumSpanningTree_ShouldReturnNoEdges_IfGraphContainsNoNodes()
        {
            var mst = _mstSearcher.GetMinimumSpanningTree(0).ToArray();

            Assert.AreEqual(0, mst.Length);
        }

        [TestMethod]
        public void GetMinimumSpanningTree_ShouldReturnNoEdges_IfGraphContainsOneNode()
        {
            var mst = _mstSearcher.GetMinimumSpanningTree(1).ToArray();

            Assert.AreEqual(0, mst.Length);
        }

        [TestMethod]
        public void GetMinimumSpanningTree_ShouldReturnOneEdge_IfGraphContainsTwoNodesWithOneEdgeBetween()
        {
            var edge = new Edge(1, 2, 5);

            var mst = _mstSearcher.GetMinimumSpanningTree(2, edge).ToArray();

            Assert.AreEqual(1, mst.Length);
            Assert.AreEqual(edge, mst[0]);
        }

        [TestMethod]
        public void GetMinimumSpanningTree_ShouldReturnOneEdge_IfGraphContainsTwoNodesWithSeveralEdgesBetween()
        {
            var edge = new Edge(1, 2, 1);

            var mst = _mstSearcher.GetMinimumSpanningTree(2, new Edge(1, 2, 3), edge, new Edge(1, 2, 5)).ToArray();

            Assert.AreEqual(1, mst.Length);
            Assert.AreEqual(edge, mst[0]);
        }

        [TestMethod]
        public void GetMinimumSpanningTree_ShouldReturnCorrectEdges_IfGraphContainsSeveralNodes()
        {
            var edge1 = new Edge(1, 2, 1);
            var edge2 = new Edge(2, 3, 2);
            var edge3 = new Edge(3, 1, 1);

            var mst = _mstSearcher.GetMinimumSpanningTree(3, edge1, edge2, edge3).ToArray();

            CollectionAssert.AreEquivalent(new[] { edge1, edge3 }, mst);
        }

        [TestMethod]
        public void GetMinimumSpanningTree_ShouldReturnCorrectEdges_IfGraphIsComplex()
        {
            var edge1 = new Edge(1, 2, 1);
            var edge2 = new Edge(2, 3, 2);
            var edge3 = new Edge(3, 4, 5);
            var edge4 = new Edge(4, 1, 4);
            var edge5 = new Edge(3, 1, 3);

            var mst = _mstSearcher.GetMinimumSpanningTree(4, edge1, edge2, edge3, edge4, edge5).ToArray();

            CollectionAssert.AreEquivalent(new[] { edge1, edge2, edge4 }, mst);
        }

        [TestMethod]
        public void GetMinimumSpanningTree_ShouldReturnCorrectEdges_IfGraphIsComplex_WithBigWeights()
        {
            var edge1 = new Edge(1, 2, 100);
            var edge2 = new Edge(2, 3, 200);
            var edge3 = new Edge(3, 4, 500);
            var edge4 = new Edge(4, 1, 400);
            var edge5 = new Edge(3, 1, 300);

            var mst = _mstSearcher.GetMinimumSpanningTree(4, edge1, edge2, edge3, edge4, edge5).ToArray();

            CollectionAssert.AreEquivalent(new[] { edge1, edge2, edge4 }, mst);
        }
    }
}
