using System;
using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.Algorithms.Graphs.Search;
using EdlinSoftware.Tests.DataStructures.Graphs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Graphs.Search
{
    [TestClass]
    public class BreadthFirstSearcherTest : GraphSearcherTestBase<BreadthFirstSearcher<GraphNode>>
    {
        [TestInitialize]
        public void TestInitiazlie()
        {
            _searcher = new BreadthFirstSearcher<GraphNode>();
            _visitedNodes = new List<GraphNode>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_ShouldThrowException_IfGraphNodeIsNull()
        {
            _searcher.Search(null, ConnectedNodesSelector, args => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_ShouldThrowException_IfNodesSelectorIsNull()
        {
            _searcher.Search(GetDirectedGraph(1)[0], null, args => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_ShouldThrowException_IfActionIsNull()
        {
            _searcher.Search(GetUndirectedGraph(1)[0], ConnectedNodesSelector, null);
        }

        [TestMethod]
        public void Search_ShouldVisitStartNode()
        {
            var startNode = GetUndirectedGraph(1)[0];

            _searcher.Search(startNode, ConnectedNodesSelector, args => _visitedNodes.Add(args.TargetNode));

            CollectionAssert.Contains(_visitedNodes, startNode);
        }

        [TestMethod]
        public void Search_ShouldVisitAllReachableNodes()
        {
            var graph = GetUndirectedGraph(5, "1-2", "2-3");

            _searcher.Search(graph[0], ConnectedNodesSelector, args => _visitedNodes.Add(args.TargetNode));

            CollectionAssert.IsSubsetOf(graph.Take(3).ToArray(), _visitedNodes);
        }

        [TestMethod]
        public void Search_ShouldNotVisitAllUnreachableNodes()
        {
            var graph = GetUndirectedGraph(5, "1-2", "2-3");

            _searcher.Search(graph[0], ConnectedNodesSelector, args => _visitedNodes.Add(args.TargetNode));

            CollectionAssert.IsNotSubsetOf(graph.Skip(3).ToArray(), _visitedNodes);
        }

        [TestMethod]
        public void Search_ShouldVisitNodesByLayers()
        {
            var graph = GetUndirectedGraph(6, "1-2", "1-3", "2-4", "2-5", "3-5", "4-6", "5-6");

            _searcher.Search(graph[0], ConnectedNodesSelector, args => _visitedNodes.Add(args.TargetNode));

            CollectionAssert.AreEqual(_visitedNodes, new[] { graph[0], graph[1], graph[2], graph[3], graph[4], graph[5] });
        }
    }
}
