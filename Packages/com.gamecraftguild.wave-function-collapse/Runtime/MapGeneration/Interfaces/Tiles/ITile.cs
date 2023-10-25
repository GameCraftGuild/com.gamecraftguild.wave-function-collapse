using System.Collections.Generic;

namespace GameCraftGuild.WaveFunctionCollapse {

    public interface ITile {

        /// <summary>
        /// Name for the tile.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Tags associated with the tile.
        /// </summary>
        public HashSet<string> Tags { get; }

        /// <summary>
        /// Connections for each side of tile. These are ordered.
        /// </summary>
        public string[] Connections { get; }

        /// <summary>
        /// The rotation for this tile.
        /// </summary>
        public int Rotation { get; }

        /// <summary>
        /// Modifiers for adjacent tile probabilities if this gets placed.
        /// </summary>
        public Dictionary<string, int> ProbabilityModifiers { get; }

    }

}