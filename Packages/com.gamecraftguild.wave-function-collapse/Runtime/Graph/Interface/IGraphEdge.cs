using System;

/// <summary>
/// Interface for edges in a graph.
/// </summary>
public interface IGraphEdge
{
    /// <summary>
    /// Node this edge is from.
    /// </summary>
    public IGraphNode From { get; }

    /// <summary>
    /// Node this edge is to.
    /// </summary>
    public IGraphNode To { get; }

    /// <summary>
    /// Is this edge directed.
    /// </summary>
    public bool Directed { get; }

    /// <summary>
    /// Get the node on this edge that is not <paramref name="node"/>.
    /// </summary>
    /// <param name="node">The node on this edge not looked for.</param>
    /// <returns><list type="bullet"><item>The node on this edge that is not <paramref name="node"/>.</item><item>Null if <paramref name="node"/> is not on this edge.</item></list></returns>
    public IGraphNode GetOtherNode(IGraphNode node);

}
