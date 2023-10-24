using System.Numerics;
using System.Collections.Generic;

public interface IMap {

    /// <summary>
    /// Get the root node for the map graph (This should be a connected graph).
    /// </summary>
    public MapNode Root { get; }

    /// <summary>
    /// All the preset tiles in the map.
    /// </summary>
    public List<IPresetTile> PresetTiles { get; }

    /// <summary>
    /// Dictionary mapping connections to a HashSet of valid connections
    /// </summary>
    public Dictionary<string, HashSet<string>> ValidTileConnections { get; }

    /// <summary>
    /// Get the node at <paramref name="coordinate"/>.
    /// </summary>
    /// <param name="coordinate"></param>
    /// <returns><list type="bullet"><item>The node at <paramref name="coordinate"/>.</item><item>Null if there is no node at <paramref name="coordinate"/>.</item></list></returns>
    public MapNode GetNodeAt(Vector3 coordinate);

    /// <summary>
    /// Get an ordered representation of the node's edges. This will be passd to the MapNodes upon creation.
    /// </summary>
    /// <param name="node">Node to order the edges for.</param>
    /// <returns></returns>
    public MapEdge[] OrderEdgesFor(MapNode node);

    /// <summary>
    /// Create the node graph. Nodes should be populated here.
    /// </summary>
    public void CreateNodes();

    /// <summary>
    /// Get the next node to collapse. Should be the node with the lowest entropy (fewest possible states).
    /// </summary>
    /// <returns><list type="bullet"><item>The next node to collapse.</item><item>Null if there are no more nodes to collapse.</item></list></returns>
    public MapNode GetNextNodeToCollapse();

}
