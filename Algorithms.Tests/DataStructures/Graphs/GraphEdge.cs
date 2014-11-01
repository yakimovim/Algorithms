using System;
using System.Diagnostics;

namespace EdlinSoftware.Tests.DataStructures.Graphs
{
    [DebuggerDisplay("Edge from {First.Id} to {Second.Id}")]
    public class GraphEdge
    {
        public GraphNode First { get; set; }

        public GraphNode Second { get; set; }

        public GraphNode GoesFrom(GraphNode node)
        {
            if (First == node)
                return Second;
            if (Second == node)
                return First;
            throw new ArgumentException("Edge is not connected to the given node.", "node");
        }
    }
}