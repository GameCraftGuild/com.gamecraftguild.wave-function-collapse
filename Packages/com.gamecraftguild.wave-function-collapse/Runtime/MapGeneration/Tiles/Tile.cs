using System;
using System.Collections.Generic;

public class Tile : ITile
{
    /// <summary>
    /// Name for the tile.
    /// </summary>
    protected string name;

    /// <inheritdoc/>
    public string Name {
        get {
            return name;
        }
    }

    /// <summary>
    /// Tags associated with the tile.
    /// </summary>
    protected HashSet<string> tags = new HashSet<string>();

    /// <inheritdoc/>
    public HashSet<string> Tags {
        get {
            return tags;
        }
    }

    /// <summary>
    /// Connections for each side of the tile. These are ordered.
    /// </summary>
    protected string[] connections;

    /// <inheritdoc/>
    public string[] Connections {
        get {
            return connections;
        }
    }

    /// <summary>
    /// The rotation for this tile.
    /// </summary>
    protected int rotation = 0;

    /// <inheritdoc/>
    public int Rotation {
        get {
            return rotation;
        }
    }

    /// <summary>
    /// Modifiers for adjacent tile probabilities if this gets placed.
    /// </summary>
    protected Dictionary<string, int> probabilityModifiers = new Dictionary<string, int>();

    /// <inheritdoc/>
    public Dictionary<string, int> ProbabilityModifiers {
        get {
            return probabilityModifiers;
        }
    }

    /// <summary>
    /// Create a new HexTile with the given info.
    /// </summary>
    /// <param name="name">Name of the tile.</param>
    /// <param name="tags">Tags associated with the tile.</param>
    /// <param name="connections">Connections for each side of the tile.</param>
    /// <param name="probabilityModifiers">Probability modifers for adjacent tile probabilities if this gets placed.</param>
    public Tile(string name, HashSet<string> tags, string[] connections, Dictionary<string, int> probabilityModifiers) {
        this.name = name;
        this.tags = tags;
        this.connections = connections;
        this.probabilityModifiers = probabilityModifiers;
    }


}
