using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using Microsoft.VisualStudio.DebuggerVisualizers;

[assembly: System.Diagnostics.DebuggerVisualizer(
typeof(EdlinSoftware.Algorithms.Visualizers.GraphNodeVisualizer),
Target = typeof(EdlinSoftware.Algorithms.Visualizers.GraphNode),
Description = "Edlin Software Graph Visualizer")]

namespace EdlinSoftware.Algorithms.Visualizers
{
    public class GraphNodeVisualizer : DialogDebuggerVisualizer
    {
        protected override void Show(IDialogVisualizerService windowService, IVisualizerObjectProvider objectProvider)
        {
            var form = new Form();

            var gViewer = new GViewer
            {
                Dock = DockStyle.Fill
            };

            form.Controls.Add(gViewer);

            Graph graph = new Graph();

            FillGraph(graph, objectProvider.GetObject() as GraphNode);

            gViewer.Graph = graph;

            windowService.ShowDialog(form);
        }

        private void FillGraph(Graph graph, GraphNode graphNode)
        {
            if(graphNode == null)
                return;

            var stack = new Stack<GraphNode>();
            stack.Push(graphNode);
            graphNode.GraphViewerNode = new Node(graphNode.Content);
            graph.AddNode(graphNode.GraphViewerNode);

            while (stack.Count > 0)
            {
                graphNode = stack.Pop();

                if (graphNode.Edges != null)
                {
                    foreach (var edge in graphNode.Edges)
                    {
                        var toNode = edge.To;

                        if (toNode.GraphViewerNode == null)
                        {
                            stack.Push(toNode);
                            toNode.GraphViewerNode = new Node(toNode.Content);
                            graph.AddNode(toNode.GraphViewerNode);
                        }

                        var graphViewerEdge = new Edge(graphNode.GraphViewerNode, toNode.GraphViewerNode, ConnectionToGraph.Connected);
                        if (!edge.IsDirected)
                        {
                            graphViewerEdge.Attr.ArrowheadAtSource = ArrowStyle.None;
                            graphViewerEdge.Attr.ArrowheadAtTarget = ArrowStyle.None;
                        }
                    }
                }
            }
        }

        public static void TestShowVisualizer(object objectToVisualize)
        {
            VisualizerDevelopmentHost visualizerHost = new VisualizerDevelopmentHost(objectToVisualize, typeof(GraphNodeVisualizer));
            visualizerHost.ShowVisualizer();
        }
    }

    [Serializable]
    public class GraphNode
    {
        public string Content { get; set; }

        public IEnumerable<GraphEdge> Edges { get; set; }

        internal Node GraphViewerNode { get; set; }
    }

    [Serializable]
    public class GraphEdge
    {
        public string Content { get; set; }

        public bool IsDirected { get; set; }

        public GraphNode From { get; set; }

        public GraphNode To { get; set; }
    }
}
