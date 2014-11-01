using System;
using System.Collections.Generic;

namespace EdlinSoftware.Algorithms.Graphs.Search
{
    /// <summary>
    /// Represents seracher of nodes in graph.
    /// </summary>
    /// <typeparam name="TGraphNode">Type of graph node.</typeparam>
    public interface IGraphSearcher<TGraphNode>
    {
        /// <summary>
        /// Clears list of visited nodes.
        /// </summary>
        void ClearVisitedNodes();

        /// <summary>
        /// Searches nodes in graph.
        /// </summary>
        /// <param name="startNode">Node to start from.</param>
        /// <param name="connectedNodesSelector">Selector of nodes reachable from given one.</param>
        /// <param name="nodeAction">Action to execute then node is found.</param>
        void Search(TGraphNode startNode, 
            Func<TGraphNode, IEnumerable<TGraphNode>> connectedNodesSelector, 
            Action<IGraphNodeVisitingArgs<TGraphNode>> nodeAction);
    }
}
