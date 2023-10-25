using System;
using System.Numerics;
using System.Collections.Generic;

namespace GameCraftGuild.WaveFunctionCollapse {

    public class HexNodeGenerationFunctions {

        public static Dictionary<string, Func<IMap, Func<HashSet<IPossibleTile>>, int, int, MapNode[,]>> nodeGenerationMapping = new Dictionary<string, Func<IMap, Func<HashSet<IPossibleTile>>, int, int, MapNode[,]>>() {
        { "Ring", CreateHexRing }
    };

        /// <summary>
        /// Create a map of nodes in a ring shape.
        /// </summary>
        /// <param name="map">Map the nodes are created for.</param>
        /// <param name="possibleTiles">Possible tiles that the nodes can be.</param>
        /// <param name="primarySize">Number of rings.</param>
        /// <param name="secondarySize">Unused.</param>
        /// <returns>2D array containing all the nodes.</returns>
        public static MapNode[,] CreateHexRing(IMap map, Func<HashSet<IPossibleTile>> possibleTiles, int primarySize, int secondarySize) {
            MapNode[,] nodes = new MapNode[primarySize * 2 + 1, primarySize * 2 + 1];

            MapNode n;
            for (int q = -primarySize; q <= primarySize; q++) {
                int r1 = Math.Max(-primarySize, -q - primarySize);
                int r2 = Math.Min(primarySize, -q + primarySize);
                for (int r = r1; r <= r2; r++) {
                    n = new MapNode(new Vector3(q + primarySize, r + primarySize, -(q + primarySize) - (r + primarySize)), map.OrderEdgesFor, map, possibleTiles());
                    nodes[(int)n.Coordinate.X, (int)n.Coordinate.Y] = n;
                }
            }

            return nodes;
        }

    }

}