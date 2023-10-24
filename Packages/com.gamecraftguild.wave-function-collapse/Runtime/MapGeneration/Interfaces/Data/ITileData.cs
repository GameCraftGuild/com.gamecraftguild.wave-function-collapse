

using System.Collections.Generic;

public interface ITileData 
{
    /// <summary>
    /// Name for the tile.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Tags associated with the tile.
    /// </summary>
    public HashSet<string> Tags { get; set; }

    /// <summary>
    /// Connections for each side of tile. These are ordered.
    /// </summary>
    public string[] Connections { get; set; }

    /// <summary>
    /// Probability for selecting this tile.
    /// </summary>
    public int Probability { get; set; }

    /// <summary>
    /// Modifiers for adjacent tile probabilities if this gets placed.
    /// </summary>
    public Dictionary<string, int> ProbabilityModifiers { get; set; }
}
