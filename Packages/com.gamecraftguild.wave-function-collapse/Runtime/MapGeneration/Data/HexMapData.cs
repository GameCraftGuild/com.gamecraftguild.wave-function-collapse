using System;

[Serializable]
public class HexMapData : IMapData {

    ///<inheritdoc/>
    public string TileListName { get; set; }

    /// <inheritdoc/>
    public string TileConnectionsName { get; set; }

    /// <inheritdoc/>
    public string PresetTilesName { get; set; }

    /// <summary>
    /// Primary size attribute used when creating the map.
    /// </summary>
    public int PrimarySize { get; set; }

    /// <summary>
    /// Secondary size attribute used when creating the map.
    /// </summary>
    public int SecondarySize { get; set; }

    /// <summary>
    /// Shape of the map; determines what node generation function to use.
    /// </summary>
    public string MapShape { get; set; }
}
