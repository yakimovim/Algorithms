using System;
using System.Collections.Generic;
using EdlinSoftware.Algorithms.Visualizers;

namespace Visualizer.Test
{
    class Program
    {
        static void Main()
        {
            var node = new TestGraphNode("A", new TestGraphNode("B"), new TestGraphNode("C"));

            GraphNodeVisualizer.TestShowVisualizer(node);
        }
    }

    [Serializable]
    public class TestGraphNode : GraphNode
    {
        public TestGraphNode(string content, params TestGraphNode[] connected)
        {
            Content = content;

            var edges = new List<GraphEdge>();

            foreach (var node in connected)
            {
                edges.Add(new GraphEdge
                {
                    From = this,
                    To = node,
                    IsDirected = true
                });
            }

            Edges = edges;
        }
    }
}
