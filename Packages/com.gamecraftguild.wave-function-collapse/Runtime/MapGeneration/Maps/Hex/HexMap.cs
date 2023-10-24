
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEditor.Experimental.GraphView;

public class HexMap : IMap
{
    /// <summary>
    /// Get the root node for the map graph (This should be a connected graph).
    /// </summary>
    private MapNode root;

    /// <inheritdoc/>
    public MapNode Root {
        get {
            return root;
        }
    }

    /// <summary>
    /// All the preset tiles in the map.
    /// </summary>
    private List<IPresetTile> presetTiles;

    /// <inheritdoc/>
    public List<IPresetTile> PresetTiles {
        get {
            return presetTiles;
        }
    }

    /// <summary>
    /// Dictionary mapping connections to a HashSet of valid connections
    /// </summary>
    private Dictionary<string, HashSet<string>> validTileConnections;

    /// <inheritdoc/>
    public Dictionary<string, HashSet<string>> ValidTileConnections {
        get {
            return validTileConnections;
        }
    }

    /// <summary>
    /// Possible tiles that nodes on this map can contain.
    /// </summary>
    private HashSet<IPossibleTile> possibleTiles = new HashSet<IPossibleTile>();

    /// <summary>
    /// Data for the map.
    /// </summary>
    private HexMapData mapData;

    /// <summary>
    /// Factory for creating tiles.
    /// </summary>
    private ITileFactory tileFactory;

    /// <summary>
    /// Data loader.
    /// </summary>
    private IDataLoader dataLoader;

    /// <summary>
    /// Array of all nodes for fast lookup.
    /// </summary>
    private MapNode[,] allNodes;

    /// <summary>
    /// Array of all nodes for fast lookup.
    /// </summary>
    public MapNode[,] AllNodes {
        get {
            return allNodes;
        }
    }

    /// <summary>
    /// Functions for creating different node arrangements
    /// </summary>
    private Dictionary<string, Func<IMap, Func<HashSet<IPossibleTile>>, int, int, MapNode[,]>> nodeGenerationFunctions;

    /// <summary>
    /// Create a new map with the given information.
    /// </summary>
    /// <param name="mapName">Name of the map to create.</param>
    /// <param name="tileFactory">Tile factory to use.</param>
    /// <param name="dataLoader">Data loader to use.</param>
    /// <param name="nodeGenerationFunctions">Dictionary of functions for creating different node arrangements.</param>
    public HexMap(string mapName, ITileFactory tileFactory, IDataLoader dataLoader, Dictionary<string, Func<IMap, Func<HashSet<IPossibleTile>>, int, int, MapNode[,]>> nodeGenerationFunctions) {
        this.tileFactory = tileFactory;
        this.dataLoader = dataLoader;
        this.nodeGenerationFunctions = nodeGenerationFunctions;

        Initialize(mapName);
    }

    /// <summary>
    /// Initialize the map and load all the data.
    /// </summary>
    /// <param name="mapName">Name of the map being created.</param>
    private void Initialize(string mapName) {
        mapData = dataLoader.LoadMapData<HexMapData>(mapName); // Must be done first.

        validTileConnections = dataLoader.LoadTileConnectionsData(mapData.TileConnectionsName);

        tileFactory.InitializeTileData(dataLoader.LoadTileData(mapData.TileListName)); // Must be done before tiles and preset tiles

        possibleTiles = tileFactory.CreateAllPossibleTiles();

        presetTiles = BuildPresetTiles(dataLoader.LoadPresetTiles(mapData.PresetTilesName).ToList());
    }

    /// <summary>
    /// Create the preset tiles based on the preset tile data.
    /// </summary>
    /// <param name="data">Preset tile data.</param>
    /// <returns>Created preset tiles.</returns>
    private List<IPresetTile> BuildPresetTiles(List<IPresetTileData> data) {
        List<IPresetTile> tiles = new List<IPresetTile>();

        foreach (IPresetTileData d in data) {
            tiles.Add(tileFactory.CreatePresetTile(d.Name, d.Coordinate, d.Rotation));
        }

        return tiles;
    }

    /// <inheritdoc/>
    public MapNode GetNextNodeToCollapse() {
        MapNode next = null;
        float entropy = float.MaxValue;

        float tempEntropy;
        foreach (MapNode node in allNodes) {
            if (node == null || node.Collapsed) continue; // If the node is null or already collapsed we don't care about it

            tempEntropy = node.GetEntropy();

            if (tempEntropy < entropy) { // If entropy for this node is lower (fewer possible states) than for saved node, make the current node the saved node.
                entropy = tempEntropy;
                next = node;
            }
        }

        return next;
    }

    /// <inheritdoc/>
    public MapNode GetNodeAt(Vector3 coordinate) {
        // Confirm coordinate is within the bounds of allNodes then return node at that location.
        if (coordinate.X >= 0 && coordinate.X < allNodes.GetLength(0) && coordinate.Y >= 0 && coordinate.Y < allNodes.GetLength(1)) return allNodes[(int)coordinate.X, (int)coordinate.Y];
        return null; // Return null if out of bounds.
    }

    /// <inheritdoc/>
    public MapEdge[] OrderEdgesFor(MapNode node) {
        MapEdge[] orderedEdges = new MapEdge[6]; // This is for a hex map so each node has 6 edges. Note, some nodes (specifically nodes along the map border) may have fewer edges. Gaps will be filled in with null.

        // Assuming point up hexes
        orderedEdges[0] = GetEdgeInDirection(node, new Vector3(1, -1, 0)); // NE
        orderedEdges[1] = GetEdgeInDirection(node, new Vector3(1, 0, -1)); // E
        orderedEdges[2] = GetEdgeInDirection(node, new Vector3(0, 1, -1)); // SE
        orderedEdges[3] = GetEdgeInDirection(node, new Vector3(-1, 1, 0)); // SW
        orderedEdges[4] = GetEdgeInDirection(node, new Vector3(-1, 0, 1)); // W
        orderedEdges[5] = GetEdgeInDirection(node, new Vector3(0, -1, 1)); // NW

        return orderedEdges;
    }

    /// <summary>
    /// Get the edge that is in <paramref name="direction"/> from <paramref name="node"/>.
    /// </summary>
    /// <param name="node">Node to get the edge from.</param>
    /// <param name="direction">Direction the edge should be in.</param>
    /// <returns><list type="bullet"><item>Edge in <paramref name="direction"/></item><item>Null if there is no node in <paramref name="direction"/>.</item></list></returns>
    private MapEdge GetEdgeInDirection(MapNode node, Vector3 direction) {
        MapNode targetNode = GetNodeAt(node.Coordinate + direction);
        if (targetNode == null) return null; // Node isn't on the map

        IEnumerable<IGraphEdge> foundEdges = node.Edges.Where(edge => edge.GetOtherNode(node) == targetNode);
        if (foundEdges.Count() == 0) return null; // Node doesn't have a connection in this direction. Shouldn't ever happen.

        return (MapEdge)foundEdges.First();
    }

    /// <inheritdoc/>
    public void CreateNodes () {
        allNodes = nodeGenerationFunctions[mapData.MapShape](this, tileFactory.CreateAllPossibleTiles, mapData.PrimarySize, mapData.SecondarySize);

        AssignNeighbors();
    }

    /// <summary>
    /// Assign all the neighbors for the nodes.
    /// </summary>
    private void AssignNeighbors () {
        Vector3[] adjacent = new Vector3[6] {
            new Vector3(1, -1, 0), // NE
            new Vector3(1, 0, -1), // E
            new Vector3(0, 1, -1), // SE
            new Vector3(-1, 1, 0), // SW
            new Vector3(-1, 0, 1), // W
            new Vector3(0, -1, 1) // NW
        };

        MapNode temp;
        foreach (MapNode node in allNodes) {
            if (node == null) continue; // If node is null, this is a blank spot in the map.
            foreach (Vector3 direction in adjacent) {
                temp = GetNodeAt(node.Coordinate + direction); // If this is null, there is no node adjacent in the current direction.
                if (temp != null) node.CreateEdgeTo(temp);
            }
        }
    }
}
