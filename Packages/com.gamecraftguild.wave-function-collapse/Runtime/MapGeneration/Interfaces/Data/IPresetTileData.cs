using System.Numerics;

namespace GameCraftGuild.WaveFunctionCollapse {

    public interface IPresetTileData {
        /// <summary>
        /// Name for the tile.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Coordinate for this preset tile.
        /// </summary>
        public Vector3 Coordinate { get; set; }

        /// <summary>
        /// The rotation for this tile.
        /// </summary>
        public int Rotation { get; set; }


    }

}