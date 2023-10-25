using System.Collections.Generic;

namespace GameCraftGuild.WaveFunctionCollapse {

    /// <summary>
    /// Interface for nodes in a graph.
    /// </summary>
    public interface IGraphNode {
        /// <summary>
        /// List of associated edges.
        /// </summary>
        public HashSet<IGraphEdge> Edges { get; }

        /// <summary>
        /// Get the edge that connects this node to <paramref name="node"/>.
        /// </summary>
        /// <param name="node">Node the returned edge should connect this node to.</param>
        /// <returns><list type="bullet"><item>The IGraphEdge connecting <paramref name="node"/> to this node.</item><item>Null if there is no such edge.</item></list></returns>
        public IGraphEdge GetEdgeTo(IGraphNode node);

        /// <summary>
        /// Create an edge to <paramref name="node"/> and add it to Edges if it doesn't already exist.
        /// </summary>
        /// <param name="node">Node to create the edge to.</param>
        /// <returns><list type="bullet"><item>The existing edge if one to <paramref name="node"/> exists.</item><item>The created edge if no edge to <paramref name="node"/> exists.</item></list></returns>
        public IGraphEdge CreateEdgeTo(IGraphNode node);

        /// <summary>
        /// Remove the edge to <paramref name="node"/>
        /// </summary>
        /// <param name="node">Node that an edge to should be removed.</param>
        /// <returns>True if:<list type="bullet">Edge to <paramref name="node"/> was removed.</list>False if:<list type="bullet"><item>The edge to <paramref name="node"/> was removed.</item><item>No edge to <paramref name="node"/> exists.</item></list></returns>
        public bool RemoveEdgeTo(IGraphNode node);

        /// <summary>
        /// Add an edge to Edges if it doesn't already exist.
        /// </summary>
        /// <param name="edge">Edge to add to edges.</param>
        /// <returns>True if: <list type="bullet"><paramref name="edge"/> was added.</list>False if: <list type="bullet"><paramref name="edge"/> was not added.</list></returns>
        public bool AddEdge(IGraphEdge edge);

        /// <summary>
        /// Remove <paramref name="edge"/> from Edges.
        /// </summary>
        /// <param name="edge">Edge to remove.</param>
        /// <returns>True if:<list type="bullet">The edge was removed.</list>False if: <list type="bullet"><paramref name="edge"/> was not removed.</list></returns>
        public bool RemoveEdge(IGraphEdge edge);

        /// <summary>
        /// Get all of the nodes accessible from this node.
        /// </summary>
        /// <returns>Nodes connected to this node via edge.</returns>
        public HashSet<IGraphNode> GetConnectedNodes();
    }

}