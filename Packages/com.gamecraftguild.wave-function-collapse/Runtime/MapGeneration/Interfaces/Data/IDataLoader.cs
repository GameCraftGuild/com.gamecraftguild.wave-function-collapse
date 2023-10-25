
using System.Collections.Generic;

namespace GameCraftGuild.WaveFunctionCollapse {

    public interface IDataLoader {
        /// <summary>
        /// Load the map data for <paramref name="mapName"/>.
        /// </summary>
        /// <param name="mapName">Name of the file to load the data from.</param>
        /// <typeparam name="T">Type of map data to load.</typeparam>
        /// <returns>Loaded map data.</returns>
        public T LoadMapData<T>(string mapName) where T : IMapData;

        /// <summary>
        /// Load the tile data.
        /// </summary>
        /// <param name="tileListName">Name of file containing the list of tile names used.</param>
        /// <returns>Dictionary mapping tile names to tile data.</returns>
        public Dictionary<string, ITileData> LoadTileData(string tileListName);

        /// <summary>
        /// Load tile connection data.
        /// </summary>
        /// <param name="tileConnectionsName">Name of file containing tile connection data.</param>
        /// <returns>Dictionary mapping connections to a HashSet of valid connections</returns>
        public Dictionary<string, HashSet<string>> LoadTileConnectionsData(string tileConnectionsName);

        /// <summary>
        /// Load the preset tiles.
        /// </summary>
        /// <param name="presetTilesName">Name of file continaing the preset tiles.</param>
        /// <returns>Array of preset tiles.</returns>
        public IPresetTileData[] LoadPresetTiles(string presetTilesName);
    }

}