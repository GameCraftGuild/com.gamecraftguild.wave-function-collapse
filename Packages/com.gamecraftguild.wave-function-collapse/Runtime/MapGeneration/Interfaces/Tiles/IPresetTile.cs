using System.Numerics;

namespace GameCraftGuild.WaveFunctionCollapse {

    public interface IPresetTile : ITile {

        /// <summary>
        /// Coordinate for this preset tile.
        /// </summary>
        public Vector3 Coordinate { get; }

    }

}