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
        public void TestInitialize()
        {
            Searcher = new BreadthFirstSearcher<GraphNode>();
            VisitedNodes = new List<GraphNode>();
        }

        [TestMethod, Owner("Ivan Yakimov")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_ShouldThrowException_IfGraphNodeIsNull()
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
        public void Search_ShouldThrowException_IfActionIsNull()
        {
            Searcher.Search(GetUndirectedGraph(1)[0], ConnectedNodesSelector, null);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_ShouldVisitStartNode()
        {
            var startNode = GetUndirectedGraph(1)[0];

            Searcher.Search(startNode, ConnectedNodesSelector, args => VisitedNodes.Add(args.TargetNode));

            CollectionAssert.Contains(VisitedNodes, startNode);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_ShouldVisitAllReachableNodes()
        {
            var graph = GetUndirectedGraph(5, "1-2", "2-3");

            Searcher.Search(graph[0], ConnectedNodesSelector, args => VisitedNodes.Add(args.TargetNode));

            CollectionAssert.IsSubsetOf(graph.Take(3).ToArray(), VisitedNodes);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_ShouldNotVisitAllUnreachableNodes()
        {
            var graph = GetUndirectedGraph(5, "1-2", "2-3");

            Searcher.Search(graph[0], ConnectedNodesSelector, args => VisitedNodes.Add(args.TargetNode));

            CollectionAssert.IsNotSubsetOf(graph.Skip(3).ToArray(), VisitedNodes);
        }

        [TestMethod, Owner("Ivan Yakimov")]
        public void Search_ShouldVisitNodesByLayers()
        {
            var graph = GetUndirectedGraph(6, "1-2", "1-3", "2-4", "2-5", "3-5", "4-6", "5-6");

            Searcher.Search(graph[0], ConnectedNodesSelector, args => VisitedNodes.Add(args.TargetNode));

            CollectionAssert.AreEqual(VisitedNodes, new[] { graph[0], graph[1], graph[2], graph[3], graph[4], graph[5] });
        }
    }
}
