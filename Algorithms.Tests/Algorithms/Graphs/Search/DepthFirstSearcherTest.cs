using System;
using System.Collections.Generic;
using EdlinSoftware.Algorithms.Graphs.Search;
using EdlinSoftware.Tests.DataStructures.Graphs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Graphs.Search
{
    [TestClass]
    public class DepthFirstSearcherTest : GraphSearcherTestBase<DepthFirstSearcher<GraphNode>>
    {
        [TestInitialize]
        public void TestInitialize()
        {
            _searcher = new DepthFirstSearcher<GraphNode>();
            _visitedNodes = new List<GraphNode>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_ShouldThrowException_IfStartNodeIsNull()
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
        public void Search_ShouldThrowException_IfNodeActionIsNull()
        {
            _searcher.Search(GetDirectedGraph(1)[0], ConnectedNodesSelector, null);
        }

        [TestMethod]
        public void Search_ShouldVisitStartNode()
        {
            var graph = GetDirectedGraph(1);

            _searcher.Search(graph[0], ConnectedNodesSelector, args => _visitedNodes.Add(args.TargetNode));

            CollectionAssert.Contains(_visitedNodes, graph[0]);
        }

        [TestMethod]
        public void Search_ShouldVisitAllReachableNodes()
        {
            var graph = GetDirectedGraph(4, "1-2", "1-3", "4-2", "4-3");

            _searcher.Search(graph[0], ConnectedNodesSelector, args => _visitedNodes.Add(args.TargetNode));

            CollectionAssert.IsSubsetOf(new[] { graph[0], graph[1], graph[2] }, _visitedNodes);
        }

        [TestMethod]
        public void Search_ShouldNotVisitAllUnreachableNodes()
        {
            var graph = GetDirectedGraph(5, "1-2", "1-3", "4-2", "4-3");

            _searcher.Search(graph[0], ConnectedNodesSelector, args => _visitedNodes.Add(args.TargetNode));

            CollectionAssert.DoesNotContain(_visitedNodes, graph[3]);
            CollectionAssert.DoesNotContain(_visitedNodes, graph[4]);
        }

        [TestMethod]
        public void Search_ShouldVisitChildrenBeforeSiblings()
        {
            var graph = GetDirectedGraph(6, "1-2", "1-3", "2-4", "2-5", "4-6", "6-5", "5-3");

            _searcher.Search(graph[0], ConnectedNodesSelector, args => _visitedNodes.Add(args.TargetNode));

            CollectionAssert.AreEqual(_visitedNodes, new[] { graph[0], graph[1], graph[3], graph[5], graph[4], graph[2] });
        }
    }
}
