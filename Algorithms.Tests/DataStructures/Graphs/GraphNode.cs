using System.Collections.Generic;
using System.Diagnostics;

namespace EdlinSoftware.Tests.DataStructures.Graphs
{
    [DebuggerDisplay("Node {Id}")]
    public class GraphNode
    {
        public int Id { get; set; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<GraphEdge> _edges = new List<GraphEdge>();

        public List<GraphEdge> Edges
        {
            [DebuggerStepThrough]
            get { return _edges; }
        }
    }
}