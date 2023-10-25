using System;
using System.Collections.Generic;

namespace GameCraftGuild.WaveFunctionCollapse {

    public class PossibleTile : Tile, IPossibleTile {
        /// <summary>
        /// Rotations that are possible for this tile.
        /// </summary>
        private HashSet<int> validRotations = new HashSet<int>();

        /// <inheritdoc/>
        public HashSet<int> ValidRotations {
            get {
                return validRotations;
            }
        }

        /// <summary>
        /// Probability for selecting this tile.
        /// </summary>
        private int probability = 0;

        /// <inheritdoc/>
        public int Probability {
            get {
                return probability;
            }
        }

        /// <summary>
        /// Can the tile be rotated anymore.
        /// </summary>
        private bool rotationLocked = false;

        /// <inheritdoc/>
        public bool RotationLocked {
            get {
                return rotationLocked;
            }
        }

        /// <summary>
        /// Create a new HexPossibleTile with the given info.
        /// </summary>
        /// <param name="name">Name of the tile.</param>
        /// <param name="tags">Tags associated with the tile.</param>
        /// <param name="connections">Connections for each side of the tile.</param>
        /// <param name="probabilityModifiers">Probability modifers for adjacent tile probabilities if this gets placed.</param>
        /// <param name="probability">Probability for selecting this tile.</param>
        public PossibleTile(string name, HashSet<string> tags, string[] connections, Dictionary<string, int> probabilityModifiers, int probability) : base(name, tags, connections, probabilityModifiers) {
            this.probability = probability;

            for (int i = 0; i < connections.Length; i++) {
                validRotations.Add(i); // Initialize valid rotations to be all possible rotations
            }
        }

        /// <inheritdoc/>
        public bool Rotate() {
            if (rotationLocked) return false;

            string[] temp = new string[connections.Length];

            for (int i = 0; i < temp.Length - 1; i++) {
                temp[i] = connections[i + 1];
            }
            temp[temp.Length - 1] = connections[0];

            connections = temp;

            rotation++;
            rotation %= connections.Length; // Cannot have more rotations than the number of connections

            return true;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="targetRotation"/> is out of range.</exception>
        public bool RotateTo(int targetRotation) {
            if (rotationLocked) return false;

            if (targetRotation >= connections.Length)
                throw new ArgumentOutOfRangeException(nameof(targetRotation), $"Trying to rotate to rotation {targetRotation} when only rotations 0 - {connections.Length - 1} are valid.");

            while (rotation != targetRotation) {
                if (!Rotate()) return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public bool RotateToAndLock(int targetRotation) {
            if (rotationLocked) return false;

            if (!RotateTo(targetRotation)) return false;

            rotationLocked = true;

            return true;
        }

        /// <inheritdoc/>
        /// <exception cref="InvalidOperationException">Thrown if rotating the tile fails.</exception>
        public HashSet<string>[] FindValidRotations(MapNode sourceNode, MapEdge[] orderedEdges, Dictionary<string, HashSet<string>> validConnectionOptions) {
            HashSet<string>[] possibleConnections = new HashSet<string>[connections.Length];
            int initialRotation = rotation;
            bool currentRotationValid = true;

            // Set up possible connections
            for (int i = 0; i < possibleConnections.Length; i++) {
                possibleConnections[i] = new HashSet<string>();
            }

            // Clear currently valid rotations
            validRotations.Clear();

            do {
                currentRotationValid = true;
                // Check tile connections against other nodes on edge.
                for (int j = 0; j < connections.Length; j++) {
                    if (orderedEdges[j] == null) continue;

                    if (!validConnectionOptions[connections[j]].Overlaps(orderedEdges[j].GetConnectionsForOther(sourceNode))) { // Check if connections on other node contain something the connection is allowed to connect to.
                        currentRotationValid = false;
                        break;
                    }
                }

                // If the current rotation is vaild then add it to validRotations and add connections to the hashset.
                if (currentRotationValid) {
                    validRotations.Add(rotation);

                    for (int k = 0; k < possibleConnections.Length; k++) {
                        possibleConnections[k].Add(connections[k]);
                    }
                }

                // Rotate the tile.
                if (!Rotate()) throw new InvalidOperationException($"Rotating the tile when finding valid rotations failed. Did rotationLock get set to true somehow? rotationLock: {rotationLocked}");
            } while (rotation != initialRotation); // Once the initial rotation is reached, all rotations have been checked.

            return possibleConnections;
        }

        /// <inheritdoc/>
        public float GetEntropy() {
            return validRotations.Count; // May make better entropy func later.
        }

        /// <inheritdoc/>
        public void ModifyProbability(int modifer) {
            probability += modifer;
        }

    }

}