using System.Collections.Generic;
using System.Numerics;
using System;
using System.Linq;

namespace GameCraftGuild.WaveFunctionCollapse {

    /// <summary>
    /// Graph node for use in a graph of the map.
    /// </summary>
    public class MapNode : IGraphNode {

        /// <summary>
        /// List of associated edges.
        /// </summary>
        private HashSet<IGraphEdge> edges = new HashSet<IGraphEdge>();

        /// <inheritdoc/>
        public HashSet<IGraphEdge> Edges {
            get {
                return edges;
            }
        }

        /// <inheritdoc/>
        public IGraphEdge GetEdgeTo(IGraphNode node) {
            // MapEdge is not directed so check if from this to node or from node to this exists.
            foreach (IGraphEdge e in edges) {
                if ((e.From == this && e.To == node) || (e.From == node && e.To == this)) return e;
            }
            return null;
        }

        /// <inheritdoc/>
        public IGraphEdge CreateEdgeTo(IGraphNode node) {

            IGraphEdge e = GetEdgeTo(node);

            if (e == null) {
                e = new MapEdge(this, node);
                this.AddEdge(e);
                node.AddEdge(e);

            }

            return e;
        }

        /// <inheritdoc/>
        public bool RemoveEdgeTo(IGraphNode node) {
            IGraphEdge e = GetEdgeTo(node);
            if (e == null) return false;

            return RemoveEdge(e);
        }

        /// <inheritdoc/>
        public bool AddEdge(IGraphEdge edge) {
            if (edge.From != this && edge.To != this) return false; // This edge doesn't connect to this node.

            if (edge is MapEdge) {
                MapEdge e = (MapEdge)edge;
                e.ChangeConnectionsFor(this, map.ValidTileConnections.Keys.ToHashSet());
            }

            return edges.Add(edge);
        }

        /// <inheritdoc/>
        public bool RemoveEdge(IGraphEdge edge) {
            return edges.Remove(edge);
        }

        /// <inheritdoc/>
        public HashSet<IGraphNode> GetConnectedNodes() {
            HashSet<IGraphNode> nodes = new HashSet<IGraphNode>();

            foreach (MapEdge e in edges) {
                nodes.Add(e.GetOtherNode(this));
            }

            return nodes;
        }

        // ======================== ^^^ Interface Implementation ^^^ ========================

        /// <summary>
        /// Coordinate for the node. Should not have nodes with duplicate coordinates so can act as a unique identifier.
        /// </summary>
        private Vector3 coordinate;

        /// <summary>
        /// Coordinate for the node. Should not have nodes with duplicate coordinates so can act as a unique identifier.
        /// </summary>
        public Vector3 Coordinate {
            get {
                return coordinate;
            }
        }

        /// <summary>
        /// Action to order all of the edges on the node. Returned array should have null in locations where there isn't an edge.
        /// </summary>
        private Func<MapNode, MapEdge[]> orderEdges;

        /// <summary>
        /// An ordered representation of the node's edges.
        /// </summary>
        private MapEdge[] orderedEdges;

        /// <summary>
        /// Has a specific tile been chosen for the node.
        /// </summary>
        private bool collapsed = false;

        /// <summary>
        /// Has a specific tile been chosen for the node.
        /// </summary>
        public bool Collapsed {
            get {
                return collapsed;
            }
        }

        /// <summary>
        /// Represents how many possible states this can be in. Lower entropy nodes will be collapsed sooner.
        /// </summary>
        private float entropy = int.MaxValue;

        /// <summary>
        /// Tile this is set to after it is collapsed.
        /// </summary>
        private ITile setTile;

        public ITile SetTile {
            get {
                return setTile;
            }
        }

        /// <summary>
        /// Possible tiles this could be.
        /// </summary>
        private HashSet<IPossibleTile> possibleTiles = new HashSet<IPossibleTile>();

        /// <summary>
        /// The map this node is a part of.
        /// </summary>
        private IMap map;

        /// <summary>
        /// Create a new MapNode.
        /// </summary>
        /// <param name="coordinate">Coordinate for the node.</param>
        /// <param name="orderEdges">Function for putting the edges in an appropriate order (needed to check valid tile rotations). Returned array should have null in locations where there isn't an edge.</param>
        /// <param name="map">The map this node is a part of.</param>
        /// <param name="possibleTiles">Possible tiles for this node.</param>
        public MapNode(Vector3 coordinate, Func<MapNode, MapEdge[]> orderEdges, IMap map, HashSet<IPossibleTile> possibleTiles) {
            this.coordinate = coordinate;
            this.orderEdges = orderEdges;
            this.map = map;
            this.possibleTiles = possibleTiles;
        }

        /// <summary>
        /// Get an ordered representation of the node's edges. This is only cached and only recalculated if the number of edges changes.
        /// </summary>
        /// <returns>Ordered representation of the node's edges.</returns>
        public MapEdge[] GetOrderedEdges() {
            if (orderedEdges == null || edges.Count != orderedEdges.Length) orderedEdges = orderEdges(this); // Possible issue if an equal number of edges are added & removed between calls 
            return orderedEdges;
        }

        /// <summary>
        /// Collapse the node into a specific tile.
        /// </summary>
        public void Collapse() {
            List<IPossibleTile> tempPossibleTiles = possibleTiles.ToList();
            IPossibleTile chosenTile = tempPossibleTiles.GetRandomFromWeightedList(tempPossibleTiles.Select(t => t.Probability).ToList()); // Chose a tile based using the probabilities.

            if (chosenTile == null)
                throw new MapGenerationException($"Failed to choose a tile for MapNode at {coordinate}. Likely due to probabilities of all possible tiles being nonpositive.");

            chosenTile.RotateToAndLock(chosenTile.ValidRotations.ToList().GetRandomFromList()); // Pick a rotation.

            setTile = chosenTile;

            collapsed = true;
            UpdateAfterCollapse();
        }

        /// <summary>
        /// Set the node to a specific tile, regardless of surroundings.
        /// </summary>
        /// <param name="preset">Data for the tile to set this to.</param>
        public void ForceSet(IPresetTile preset) {
            collapsed = true; // This counts as collapsing the node into a specific tile.
            setTile = preset;
            UpdateAfterCollapse();
        }

        /// <summary>
        /// Assign connections to the edges in the proper order.
        /// </summary>
        /// <param name="connections">Array of new connections.</param>
        /// <returns>List of nodes where the connections changed. Need to propogate changes to these nodes.</returns>
        /// <exception cref="InvalidOperationException">Thrown if <see cref="orderedEdges"/> length doesn't match <paramref name="connections"/> length. Likely cause if <see cref="orderEdges"/> does not properly add null values where there are missing edges.</exception>
        private List<MapNode> ChangeConnectionsForEdges(HashSet<string>[] connections) {
            List<MapNode> toUpdate = new List<MapNode>();

            GetOrderedEdges(); // Make sure orderedEdges is current

            if (orderedEdges.Length != connections.Length) {
                throw new InvalidOperationException($"MapNode: orderedEdges length ({orderedEdges.Length}) does not match connections length ({connections.Length}). Does the orderEdges function properly add null values where there are missing edges?");
            }

            for (int i = 0; i < orderedEdges.Length; i++) {
                if (orderedEdges[i] == null) continue; // No edge at this position.
                if (orderedEdges[i].ChangeConnectionsFor(this, connections[i])) // Set the connection for the edge to the appropriate value from the tile. If this changes the connections then:
                    toUpdate.Add((MapNode)orderedEdges[i].GetOtherNode(this)); // Add the other node on the edge to be updated.
            }

            return toUpdate;
        }

        /// <summary>
        /// Update the connections to adjacent nodes after the tile has collapsed to a set tile then propogate the changes to the adjacent nodes. Also update tile probabilites on adjacent tiles.
        /// </summary>
        private void UpdateAfterCollapse() {
            edges.Select(edge => edge.GetOtherNode(this)).ToList().ForEach(node => ((MapNode)node).UpdateTileProbabilities(setTile.ProbabilityModifiers)); // Set probabilites for tiles on adjacent nodes.

            List<MapNode> toUpdate = ChangeConnectionsForEdges(setTile.Connections.Select(c => new HashSet<string>() { c }).ToArray()); // Update connections to adjacent nodes


            foreach (MapNode node in toUpdate) {
                node.PropogateChanges(); // Propogate changes to nodes where the connections changed
            }

        }

        /// <summary>
        /// Update self possibilities 
        /// </summary>
        /// <exception cref="MapGenerationException">Thrown if there are no possible valid tiles for a node. This means generation has failed.</exception>
        private void PropogateChanges() {
            if (collapsed) return; // Already collapsed, no need to make any changes.

            List<MapNode> toUpdate;
            List<IPossibleTile> toRemove = new List<IPossibleTile>();

            GetOrderedEdges(); // Make sure orderedEdges are up to date.

            HashSet<string>[] newConnections = new HashSet<string>[orderedEdges.Length];
            HashSet<string>[] tempConnections;

            // Initialize new connections
            for (int i = 0; i < newConnections.Length; i++) {
                newConnections[i] = new HashSet<string>();
            }

            // Valid possibilities for each tile.
            foreach (IPossibleTile possibleTile in possibleTiles) {
                tempConnections = possibleTile.FindValidRotations(this, GetOrderedEdges(), map.ValidTileConnections); // Get the valid connection possibilities for each side with this tile
                if (possibleTile.ValidRotations.Count == 0) { // No valid rotations, mark tile to remove and continue
                    toRemove.Add(possibleTile);
                    continue; // No connections to add to list of new connections so continue the loop
                }

                for (int i = 0; i < tempConnections.Length; i++) {
                    newConnections[i].UnionWith(tempConnections[i]); // Add the connection possibilities for each side with this tile into the array with all of the new connections
                }
            }

            // Remove any tile with no valid rotations
            foreach (IPossibleTile pt in toRemove) {
                possibleTiles.Remove(pt);
            }

            // No possible options for this node. This means generation failed.
            if (possibleTiles.Count == 0) {
                throw new MapGenerationException($"MapNode at {coordinate} has no possible valid tiles.");
            }

            // Assign updated connections
            toUpdate = ChangeConnectionsForEdges(newConnections);

            // Propogate changes for nodes where the connections changed
            foreach (MapNode node in toUpdate) {
                node.PropogateChanges();
            }
        }

        /// <summary>
        /// Get the entropy of the node by summing the entropy across all possible tiles. Lower entropy means fewer possible states for the tile.
        /// </summary>
        /// <returns>The entropy of the node.</returns>
        public float GetEntropy() {
            entropy = 0;
            foreach (IPossibleTile possibleTile in possibleTiles) {
                entropy += possibleTile.GetEntropy();
            }

            return entropy;
        }

        /// <summary>
        /// Modify the probability of possible tiles on this tile.
        /// </summary>
        /// <param name="probabilityModifiers">Map of tile names to modifiers.</param>
        private void UpdateTileProbabilities(Dictionary<string, int> probabilityModifiers) {
            foreach (IPossibleTile possibleTile in possibleTiles) {
                if (probabilityModifiers.ContainsKey(possibleTile.Name))
                    possibleTile.ModifyProbability(probabilityModifiers[possibleTile.Name]);
            }
        }
    }

}