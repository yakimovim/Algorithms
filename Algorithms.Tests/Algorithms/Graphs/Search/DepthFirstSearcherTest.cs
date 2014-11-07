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
            Searcher = new DepthFirstSearcher<GraphNode>();
            VisitedNodes = new List<GraphNode>();
        }

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_ShouldThrowException_IfStartNodeIsNull()
        {
            Searcher.Search(null, ConnectedNodesSelector, args => { });
        }

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_ShouldThrowException_IfNodesSelectorIsNull()
        {
            Searcher.Search(GetDirectedGraph(1)[0], null, args => { });
        }

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_ShouldThrowException_IfNodeActionIsNull()
        {
            Searcher.Search(GetDirectedGraph(1)[0], ConnectedNodesSelector, null);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_ShouldVisitStartNode()
        {
            var graph = GetDirectedGraph(1);

            Searcher.Search(graph[0], ConnectedNodesSelector, args => VisitedNodes.Add(args.TargetNode));

            CollectionAssert.Contains(VisitedNodes, graph[0]);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_ShouldVisitAllReachableNodes()
        {
            var graph = GetDirectedGraph(4, "1-2", "1-3", "4-2", "4-3");

            Searcher.Search(graph[0], ConnectedNodesSelector, args => VisitedNodes.Add(args.TargetNode));

            CollectionAssert.IsSubsetOf(new[] { graph[0], graph[1], graph[2] }, VisitedNodes);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_ShouldNotVisitAllUnreachableNodes()
        {
            var graph = GetDirectedGraph(5, "1-2", "1-3", "4-2", "4-3");

            Searcher.Search(graph[0], ConnectedNodesSelector, args => VisitedNodes.Add(args.TargetNode));

            CollectionAssert.DoesNotContain(VisitedNodes, graph[3]);
            CollectionAssert.DoesNotContain(VisitedNodes, graph[4]);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_ShouldVisitChildrenBeforeSiblings()
        {
            var graph = GetDirectedGraph(6, "1-2", "1-3", "2-4", "2-5", "4-6", "6-5", "5-3");

            Searcher.Search(graph[0], ConnectedNodesSelector, args => VisitedNodes.Add(args.TargetNode));

            CollectionAssert.AreEqual(VisitedNodes, new[] { graph[0], graph[1], graph[3], graph[5], graph[4], graph[2] });
        }
    }
}
