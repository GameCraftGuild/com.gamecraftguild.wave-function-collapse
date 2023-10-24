using System;
using System.Collections.Generic;

public class JsonLoader : IDataLoader {

    /// <summary>
    /// Location for the map file.
    /// </summary>
    private string mapLocation = "";

    /// <summary>
    /// Location for the preset tiles file.
    /// </summary>
    private string presetTileLocation = "";

    /// <summary>
    /// Location for the tile list file.
    /// </summary>
    private string tileListLocation = "";

    /// <summary>
    /// Location for the tile data files.
    /// </summary>
    private string tileDataLocation = "";

    /// <summary>
    /// Location for the tile connections file.
    /// </summary>
    private string tileConnectionsLocation = "";

    /// <summary>
    /// Wrapper class for loading tile names.
    /// </summary>
    [Serializable]
    public class TileNamesWrapper {
        public string[] Names;
    }

    /// <summary>
    /// Wrapper class for loading tile connections
    /// </summary>
    [Serializable]
    public class TileConnectionsWrapper {
        public Dictionary<string, HashSet<string>> Connections;
    }

    /// <summary>
    /// Wrapper class for loading preset tiles.
    /// </summary>
    [Serializable]
    public class PresetTileWrapper {
        public PresetTileData[] PresetTiles;
    }

    /// <summary>
    /// Tile names
    /// </summary>
    private string[] names;

    /// <inheritdoc/>
    public T LoadMapData<T>(string mapName) where T : IMapData {
        return GenericJson.LoadFromFile<T>(mapLocation + mapName);
    }

    /// <inheritdoc/>
    public IPresetTileData[] LoadPresetTiles(string presetTilesName) {
        return GenericJson.LoadFromFile<PresetTileWrapper>(presetTileLocation + presetTilesName).PresetTiles;
    }

    /// <inheritdoc/>
    public Dictionary<string, HashSet<string>> LoadTileConnectionsData(string tileConnectionsName) {
        return GenericJson.LoadFromFile<TileConnectionsWrapper>(tileConnectionsLocation + tileConnectionsName).Connections;
    }

    /// <inheritdoc/>
    public Dictionary<string, ITileData> LoadTileData(string tileListName) {
        LoadTileNames(tileListName);

        Dictionary<string, ITileData> tileData = new Dictionary<string, ITileData>();

        foreach (string name in names) {
            tileData.Add(name, GenericJson.LoadFromFile<TileData>(tileDataLocation + name));
        }

        return tileData;
    }

    /// <summary>
    /// Load the list of tile names.
    /// </summary>
    /// <param name="tileListName">Name of the file containing the list of tile names used.</param>
    private void LoadTileNames (string tileListName) {
        names = GenericJson.LoadFromFile<TileNamesWrapper>(tileListLocation + tileListName).Names;
    }

    public JsonLoader(string mapLocation, string presetTileLocation, string tileListLocation, string tileDataLocation, string tileConnectionsLocation) {
        this.mapLocation = mapLocation;
        this.presetTileLocation = presetTileLocation;
        this.tileListLocation = tileListLocation;
        this.tileDataLocation = tileDataLocation;
        this.tileConnectionsLocation = tileConnectionsLocation;
    }
}
