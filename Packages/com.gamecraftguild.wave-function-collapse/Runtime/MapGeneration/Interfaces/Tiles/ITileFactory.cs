using System.Collections.Generic;
using System.Numerics;

namespace GameCraftGuild.WaveFunctionCollapse {

    public interface ITileFactory {
        /// <summary>
        /// Create a new tile.
        /// </summary>
        /// <param name="name">Name of the tile to create.</param>
        /// <returns>Created tile.</returns>
        public ITile CreateTile(string name);

        /// <summary>
        /// Create a new preset tile.
        /// </summary>
        /// <param name="name">Name of the tile to create.</param>
        /// <param name="coordinate">Coordinate for the preset tile.</param>
        /// <param name="rotation">Rotation of the preset tile.</param>
        /// <returns>Created preset tile.</returns>
        public IPresetTile CreatePresetTile(string name, Vector3 coordinate, int rotation);

        /// <summary>
        /// Create a new possible tile.
        /// </summary>
        /// <param name="name">Name of the tile to create.</param>
        /// <returns>Created possible tile.</returns>
        public IPossibleTile CreatePossibleTile(string name);

        /// <summary>
        /// Initialize the tile data for the factory.
        /// </summary>
        /// <param name="tileData">Tile data to be used when creating tiles.</param>
        public void InitializeTileData(Dictionary<string, ITileData> tileData);

        /// <summary>
        /// Create all possible tiles based of data loaded in <see cref="InitializeTileData(Dictionary{string, ITileData})"/>.
        /// </summary>
        /// <returns>List containing all possible tiles.</returns>
        public HashSet<IPossibleTile> CreateAllPossibleTiles();

    }

}