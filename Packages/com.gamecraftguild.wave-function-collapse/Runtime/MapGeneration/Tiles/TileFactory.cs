using System.Collections.Generic;
using System.Numerics;

namespace GameCraftGuild.WaveFunctionCollapse {

    public class TileFactory : ITileFactory {
        /// <summary>
        /// Tile data for all tiles this factory can create.
        /// </summary>
        private Dictionary<string, ITileData> tileData = new Dictionary<string, ITileData>();

        /// <inheritdoc/>
        public void InitializeTileData(Dictionary<string, ITileData> tileData) {
            this.tileData = tileData;
        }

        /// <inheritdoc/>
        public ITile CreateTile(string name) {
            ITileData data = tileData[name];
            return new Tile(data.Name, data.Tags, data.Connections, data.ProbabilityModifiers);
        }

        /// <inheritdoc/>
        public IPresetTile CreatePresetTile(string name, Vector3 coordinate, int rotation) {
            ITileData data = tileData[name];
            return new PresetTile(data.Name, data.Tags, data.Connections, data.ProbabilityModifiers, coordinate, rotation);
        }

        /// <inheritdoc/>
        public IPossibleTile CreatePossibleTile(string name) {
            ITileData data = tileData[name];
            return new PossibleTile(data.Name, data.Tags, data.Connections, data.ProbabilityModifiers, data.Probability);
        }

        /// <inheritdoc/>
        public HashSet<IPossibleTile> CreateAllPossibleTiles() {
            HashSet<IPossibleTile> possibleTiles = new HashSet<IPossibleTile>();

            foreach (string name in tileData.Keys) {
                possibleTiles.Add(CreatePossibleTile(name));
            }

            return possibleTiles;
        }

    }

}