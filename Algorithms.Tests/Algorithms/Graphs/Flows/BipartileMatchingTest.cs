using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.Algorithms.Graphs.Flows;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Graphs.Flows
{
    [TestClass]
    public class BipartileMatchingTest
    {
        [TestMethod, Owner("Ivan Yakimov")]
        public void GetMatches_Values()
        {
            CollectionAssert.AreEqual(new[] { -1 }, GetMatches(1, 1, "0"));
            CollectionAssert.AreEqual(new[] { 1 }, GetMatches(1, 1, "1"));
            CollectionAssert.AreEqual(new[] { -1, -1 }, GetMatches(2, 2, "0 0", "0 0"));
            CollectionAssert.AreEqual(new [] { 1, 2, -1 }, GetMatches(3, 4, "1 1 0 1", "0 1 0 0", "0 0 0 0"));
            CollectionAssert.AreEqual(new [] { 2, 1 }, GetMatches(2, 2, "1 1", "1 0"));
        }


        private ICollection GetMatches(int leftNodes, int rightNodes, params string[] connections)
        {
            var edges = new List<int[]>();

            foreach (var connection in connections)
            {
                var parts = connection.Split(' ').Select(int.Parse).ToArray();

                var nodeConnections = new List<int>();

                for (int i = 0; i < parts.Length; i++)
                {
                    if(parts[i] != 0)
                        nodeConnections.Add(i);
                }

                edges.Add(nodeConnections.ToArray());
            }

            var matches = new BipartileMatching<int,int>().GetMatches(Enumerable.Range(1, leftNodes), Enumerable.Range(1, rightNodes), edges);

            return matches.OrderBy(m => m.LeftSideNode).Select(m => m.HasNoMatchOnRightSide ? -1 : m.RightSideNode).ToArray();
        }

    }
}
