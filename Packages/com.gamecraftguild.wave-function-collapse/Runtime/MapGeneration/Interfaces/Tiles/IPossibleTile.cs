using System.Collections.Generic;

namespace GameCraftGuild.WaveFunctionCollapse {

    public interface IPossibleTile : ITile {
        /// <summary>
        /// Rotations that are possible for this tile.
        /// </summary>
        public HashSet<int> ValidRotations { get; }

        /// <summary>
        /// Probability for selecting this tile.
        /// </summary>
        public int Probability { get; }

        /// <summary>
        /// Can the tile be rotated anymore.
        /// </summary>
        public bool RotationLocked { get; }

        /// <summary>
        /// Rotate the connections
        /// </summary>
        /// <returns>True if:<list type="bullet">The rotation was done.</list>False if:<list type="bullet">The rotaion was not done (likely because rotation was locked; see <see cref="RotationLocked"/>).</list></returns>
        public bool Rotate();

        /// <summary>
        /// Rotate the tile until it's rotation reaches <paramref name="targetRotation"/>.
        /// </summary>
        /// <param name="targetRotation">Target rotation for the tile.</param>
        /// <returns>True if:<list type="bullet">The rotation was reached.</list>False if:<list type="bullet">The rotaion was not reached (likely because rotation was locked; see <see cref="RotationLocked"/>).</list>If false is returned, the current rotation is undefined but guaranteed to be between 0 and <see cref="ITile.Connections"/> length - 1.</returns>
        public bool RotateTo(int targetRotation);

        /// <summary>
        /// Rotate the tuile until it's rotation reaches <paramref name="targetRotation"/> and lock the rotation via <see cref="RotationLocked"/>.
        /// </summary>
        /// <param name="targetRotation">Target rotation for the tile.</param>
        /// <returns>True if:<list type="bullet">The rotation was reached and rotationLock was set to true.</list>False if:<list type="bullet">The rotaion was not reached (likely because rotation was already locked; see <see cref="RotationLocked"/>).</list>If false is returned, the current rotation is undefined but guaranteed to be between 0 and <see cref="ITile.Connections"/> length - 1. Additionally, rotationLocked is not guaranteed to be set to true.</returns>
        public bool RotateToAndLock(int targetRotation);

        /// <summary>
        /// Find the valid rotations for each tile and return the resulting possible connections along each edge.
        /// </summary>
        /// <param name="sourceNode">Node the tile is on.</param>
        /// <param name="orderedEdges">Edges connecting to this node in an appropriate order.</param>
        /// <param name="validConnectionOptions">Dictionary mapping connections to a HashSet of valid connections.</param>
        /// <returns>Possible connections along each edge.</returns>
        public HashSet<string>[] FindValidRotations(MapNode sourceNode, MapEdge[] orderedEdges, Dictionary<string, HashSet<string>> validConnectionOptions);

        /// <summary>
        /// Get the entropy associated with this tile. Lower entropy means fewer possible rotations (<see cref="ValidRotations"/>) and a lower probability (<see cref="Probability"/>)for this tile.
        /// </summary>
        /// <returns>Float representation of the entropy of this tile.</returns>
        public float GetEntropy();

        /// <summary>
        /// Modify the probability for this tile based on the modifier.
        /// </summary>
        /// <param name="modifier">Amount to modify the probability of this tile by.</param>
        public void ModifyProbability(int modifier);
    }

}