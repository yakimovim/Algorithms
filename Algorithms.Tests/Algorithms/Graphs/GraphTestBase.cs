using System;
using System.Collections.Generic;
using System.Linq;
using EdlinSoftware.DataStructures.Graphs;
using EdlinSoftware.Tests.DataStructures.Graphs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EdlinSoftware.Tests.Algorithms.Graphs
{
    [TestClass]
    public abstract class GraphTestBase
    {
        protected readonly Func<GraphNode, IEnumerable<GraphNode>> ConnectedNodesSelector = n => n.Edges.Select(e => e.GoesFrom(n)).ToArray();

        protected GraphNode[] GetUndirectedGraph(int numberOfNodes, params string[] edges)
        {
            var nodes = new List<GraphNode>();

            for (int i = 0; i < numberOfNodes; i++)
            {
                nodes.Add(new GraphNode { Id = i + 1 });
            }

            foreach (var edge in edges)
            {
                var parts = edge.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

                var firstNode = nodes[int.Parse(parts[0]) - 1];
                var secondNode = nodes[int.Parse(parts[1]) - 1];

                var graphEdge = new GraphEdge { First = firstNode, Second = secondNode };

                firstNode.Edges.Add(graphEdge);
                if (firstNode.Id != secondNode.Id)
                { secondNode.Edges.Add(graphEdge); }
            }

            return nodes.ToArray();
        }

        protected GraphNode[] GetDirectedGraph(int numberOfNodes, params string[] edges)
        {
            var nodes = new List<GraphNode>();

            for (int i = 0; i < numberOfNodes; i++)
            {
                nodes.Add(new GraphNode { Id = i + 1 });
            }

            foreach (var edge in edges)
            {
                var parts = edge.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

                var firstNode = nodes[int.Parse(parts[0]) - 1];
                var secondNode = nodes[int.Parse(parts[1]) - 1];

                var graphEdge = new GraphEdge { First = firstNode, Second = secondNode };

                firstNode.Edges.Add(graphEdge);
            }

            return nodes.ToArray();
        }

        protected IValuedEdge<long, double>[] GetDirectedValuedEdges(params string[] edgesDefinitions)
        {
            return GetValuedEdges(initializer => ValuedEdge<long, double>.Directed(initializer: initializer), edgesDefinitions);
        }

        protected IValuedEdge<long, double>[] GetUndirectedValuedEdges(params string[] edgesDefinitions)
        {
            return GetValuedEdges(initializer => ValuedEdge<long, double>.Undirected(initializer: initializer), edgesDefinitions);
        }

        private IValuedEdge<long, double>[] GetValuedEdges(Func<Action<IValuedEdge<long, double>>, IValuedEdge<long, double>> edgeGenerator, params string[] edgesDefinitions)
        {
            return edgesDefinitions
                .Select(edgeDefinition => edgeDefinition.Split(' '))
                .Select(parts => edgeGenerator(e => ValuedEdgeInitializer(e, parts)))
                .ToArray();
        }

        private void ValuedEdgeInitializer(IValuedEdge<long, double> edge, string[] definitionParts)
        {
            edge.End1 = long.Parse(definitionParts[0]) - 1;
            edge.Value = double.Parse(definitionParts[1]);
            edge.End2 = long.Parse(definitionParts[2]) - 1;
        }
    }
}
