using System;
using System.Collections.Generic;
using System.Linq;

namespace EdlinSoftware.Algorithms.Graphs.Flows
{
    /// <summary>
    /// Represents finder of maximum bipartile matching.
    /// </summary>
    public class BipartileMatching<TLeftSideNode, TRightSideNode>
    {
        private abstract class NetworkNode
        {}

        private class SourceNetworkNode : NetworkNode
        {
            public override bool Equals(object obj)
            {
                return obj is SourceNetworkNode;
            }

            public override int GetHashCode()
            {
                return "source".GetHashCode();
            }
        }

        private class SinkNetworkNode : NetworkNode
        {
            public override bool Equals(object obj)
            {
                return obj is SinkNetworkNode;
            }

            public override int GetHashCode()
            {
                return "sink".GetHashCode();
            }
        }

        private class LeftNetworkNode : NetworkNode
        {
            public readonly TLeftSideNode Node;

            public LeftNetworkNode(TLeftSideNode node)
            {
                Node = node;
            }

            public override bool Equals(object obj)
            {
                var other = obj as LeftNetworkNode;

                return (other != null) && Node.Equals(other.Node);
            }

            public override int GetHashCode()
            {
                return "left".GetHashCode() + Node.GetHashCode();
            }
        }


        private class RightNetworkNode : NetworkNode
        {
            public readonly TRightSideNode Node;

            public RightNetworkNode(TRightSideNode node)
            {
                Node = node;
            }

            public override bool Equals(object obj)
            {
                var other = obj as RightNetworkNode;

                return (other != null) && Node.Equals(other.Node);
            }

            public override int GetHashCode()
            {
                return "right".GetHashCode() + Node.GetHashCode();
            }
        }

        public IEnumerable<BipartileMatch<TLeftSideNode, TRightSideNode>> GetMatches(
            IEnumerable<TLeftSideNode> leftSideNodes,
            IEnumerable<TRightSideNode> rightSideNodes,
            IEnumerable<int[]> edges)
        {

            var leftNodes = leftSideNodes.Select(n => new LeftNetworkNode(n)).ToArray();
            var rightNodes = rightSideNodes.Select(n => new RightNetworkNode(n)).ToArray();

            var sourceNode = new SourceNetworkNode();
            var sinkNode = new SinkNetworkNode();

            var networkEdges = new Dictionary<TLeftSideNode, List<NetworkEdge<NetworkNode>>>();

            var index = 0;
            foreach (var leftNodeEdges in edges)
            {
                var leftNode = leftNodes[index++];

                foreach (var rightNodeIndex in leftNodeEdges)
                {
                    networkEdges.AddToDictionary(leftNode.Node, new NetworkEdge<NetworkNode>(1, rightNodes[rightNodeIndex]));
                }
            }

            Func<NetworkNode, IEnumerable<NetworkEdge<NetworkNode>>> edgesProvider = node =>
            {
                if (node is SourceNetworkNode)
                    return leftNodes.Select(n => new NetworkEdge<NetworkNode>(1, n)).ToArray();
                if (node is LeftNetworkNode)
                {
                    var leftNode = (LeftNetworkNode) node;
                    if (networkEdges.ContainsKey(leftNode.Node))
                        return networkEdges[leftNode.Node];
                }
                if (node is RightNetworkNode)
                {
                    return new[] {new NetworkEdge<NetworkNode>(1, sinkNode)};
                }
                return new NetworkEdge<NetworkNode>[0];
            };

            var flowFinder = new EdmondsKarpMaxFlowFinder<NetworkNode, NetworkEdge<NetworkNode>>();

            flowFinder.GetMaximumFlow(new[] {sourceNode}, new[] {sinkNode}, edgesProvider);

            foreach (var leftNode in leftNodes)
            {
                if(!networkEdges.ContainsKey(leftNode.Node))
                    yield return new BipartileMatch<TLeftSideNode, TRightSideNode>(leftNode.Node);
                else
                {
                    var edge = networkEdges[leftNode.Node].FirstOrDefault(e => e.Flow > 0);
                    if( edge == null)
                        yield return new BipartileMatch<TLeftSideNode, TRightSideNode>(leftNode.Node);
                    else
                        yield return new BipartileMatch<TLeftSideNode, TRightSideNode>(leftNode.Node, ((RightNetworkNode)edge.To).Node);
                }
            }
        }
    }

    public sealed class BipartileMatch<TLeftSideNode, TRightSideNode>
    {
        public BipartileMatch(TLeftSideNode leftSideNode)
        {
            LeftSideNode = leftSideNode;
            HasNoMatchOnRightSide = true;
        }

        public BipartileMatch(TLeftSideNode leftSideNode, TRightSideNode rightSideNode)
        {
            LeftSideNode = leftSideNode;
            RightSideNode = rightSideNode;
        }

        public TLeftSideNode LeftSideNode { get; }
        public TRightSideNode RightSideNode { get; }
        public bool HasNoMatchOnRightSide { get; }

    }
}
