using System.Collections.Generic;
using EdlinSoftware.Algorithms.Graphs.Flows;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Graphs.Flows
{
    [TestClass]
    public class EdmondsKarpMaxFlowFinderTest
    {
        [TestMethod, Owner("Ivan Yakimov")]
        public void GetMaximumFlow_Values()
        {
            Assert.AreEqual(0UL, GetMaximumFlow(1, 3, "1 2 2"));
            Assert.AreEqual(2UL, GetMaximumFlow(1, 3, "1 2 1", "1 2 1", "2 3 2"));
            Assert.AreEqual(1UL, GetMaximumFlow(1, 2, "1 2 1", "1 1 1"));
            Assert.AreEqual(20000UL, GetMaximumFlow(1, 4, "1 2 10000", "1 3 10000", "2 3 1", "3 4 10000", "2 4 10000"));
            Assert.AreEqual(6UL, GetMaximumFlow(1, 5, "1 2 2", "2 5 5", "1 3 6", "3 4 2", "4 5 1", "3 2 3", "2 4 1"));
        }

        private ulong GetMaximumFlow(ulong sourceNode, ulong sinkNode, params string[] edges)
        {
            var networkEdges = new Dictionary<ulong, List<NetworkEdge<ulong>>>();

            foreach (var edge in edges)
            {
                var data = edge.Split(' ');

                var from = ulong.Parse(data[0]);
                var to = ulong.Parse(data[1]);
                var capacity = ulong.Parse(data[2]);

                networkEdges.AddToDictionary(from, new NetworkEdge<ulong>(capacity, to));
            }

            var maxFlowFinder = new EdmondsKarpMaxFlowFinder<ulong, NetworkEdge<ulong>>();

            var maxFlow = maxFlowFinder.GetMaximumFlow(
                new[] {sourceNode},
                new[] {sinkNode},
                node =>
                {
                    if (networkEdges.ContainsKey(node))
                        return networkEdges[node];
                    return new List<NetworkEdge<ulong>>();
                }
            );

            var sourceFlow = 0UL;
            foreach (var nodeEdges in networkEdges)
            {
                var node = nodeEdges.Key;
                var outEdges = nodeEdges.Value;

                foreach (var edge in outEdges)
                {
                    if (node == sourceNode)
                        sourceFlow += edge.Flow;
                    if (edge.To == sourceNode)
                        sourceFlow -= edge.Flow;
                }
            }

            Assert.AreEqual(maxFlow, sourceFlow);

            return maxFlow;
        }
    }
}