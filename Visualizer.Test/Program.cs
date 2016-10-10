using System;
using System.Collections.Generic;
using EdlinSoftware.Algorithms.Visualizers;

namespace Visualizer.Test
{
    class Program
    {
        static void Main()
        {
            var nodeA = new TestGraphNode("A");
            var nodeB = new TestGraphNode("B");
            var nodeC = new TestGraphNode("C");

            nodeA.AddNode(nodeB);
            nodeB.AddNode(nodeC);
            nodeC.AddNode(nodeA);

            GraphNodeVisualizer.TestShowVisualizer(nodeA);
        }
    }

    [Serializable]
    public class TestGraphNode : GraphNode
    {
        private readonly List<GraphEdge> _edges;

        public TestGraphNode(string content, params TestGraphNode[] connected)
        {
            Content = content;

            _edges = new List<GraphEdge>();

            foreach (var node in connected)
            {
                AddNode(node);
            }

            Edges = _edges;
        }

        public void AddNode(TestGraphNode node)
        {
            var edge = new GraphEdge
            {
                From = this,
                To = node,
                IsDirected = true
            };
            _edges.Add(edge);
        }
    }
}
