using System.Collections.Generic;
using System.Numerics;

namespace GameCraftGuild.WaveFunctionCollapse {

    public class PresetTile : Tile, IPresetTile {
        /// <summary>
        /// Coordinate for this preset tile.
        /// </summary>
        private Vector3 coordinate;

        /// <inheritdoc/>
        public Vector3 Coordinate {
            get {
                return coordinate;
            }
        }

        /// <summary>
        /// Create a new HexPresetTile with the given info.
        /// </summary>
        /// <param name="name">Name for the tile.</param>
        /// <param name="tags">Tags associated with the tile.</param>
        /// <param name="connections">Connections for each side of the tile.</param>
        /// <param name="probabilityModifiers">Probability modifers for adjacent tile probabilities if this gets placed.</param>
        /// <param name="coordinate">Coordinate for this preset tile.</param>
        /// <param name="rotation">Preset rotation for this tile.</param>
        public PresetTile(string name, HashSet<string> tags, string[] connections, Dictionary<string, int> probabilityModifiers, Vector3 coordinate, int rotation) : base(name, tags, connections, probabilityModifiers) {
            this.coordinate = coordinate;
            this.rotation = rotation;
        }
    }

}