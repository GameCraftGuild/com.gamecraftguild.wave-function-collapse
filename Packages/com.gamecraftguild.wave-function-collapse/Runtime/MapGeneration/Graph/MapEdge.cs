using System;
using System.Collections.Generic;

namespace GameCraftGuild.WaveFunctionCollapse {

    /// <summary>
    /// Graph edge for use in a graph of the map.
    /// </summary>
    public class MapEdge : IGraphEdge, IEquatable<MapEdge> {

        /// <summary>
        /// Node this edge is from.
        /// </summary>
        private IGraphNode from;

        /// <inheritdoc/>
        public IGraphNode From {
            get {
                return from;
            }
        }

        /// <summary>
        /// Node this edge is to.
        /// </summary>
        private IGraphNode to;

        /// <inheritdoc/>
        public IGraphNode To {
            get {
                return to;
            }
        }

        /// <inheritdoc/>
        public bool Directed {
            get {
                return false; // Map nodes are not directed.
            }
        }

        /// <summary>
        /// Create a new MapEdge.
        /// </summary>
        /// <param name="from">Node this edge is from.</param>
        /// <param name="to">Node this edge is to.</param>
        public MapEdge(IGraphNode from, IGraphNode to) {
            this.from = from;
            this.to = to;
        }

        /// <inheritdoc/>
        public IGraphNode GetOtherNode(IGraphNode node) {
            if (from != node && to != node) return null; // Node is not on this edge, so getting the other node doesn't make sense.

            if (from == node) return to;
            return from;
        }

        /// <summary>
        /// Equality operator for MapEdges. MapEdges are not directed so opposite from/to is still equal.
        /// </summary>
        /// <param name="a">First MapEdge to compare.</param>
        /// <param name="b">Second MapEdge to compare.</param>
        /// <returns>True if:<list type="bullet">The MapEdges are equal.</list>False if:<list type="bullet">The MapEdges are not equal.</list></returns>
        public static bool operator ==(MapEdge a, MapEdge b) {
            if (a is null && b is null) return true; // If they are both null, should be true.
            if (a is null || b is null) return false; // But if only one is null, should be false.

            if (a.To == b.To && a.From == b.From) return true;
            if (a.To == b.From && a.From == b.To) return true; // MapEdges are not directed so opposite from/to is still equal.

            return false;
        }

        /// <summary>
        /// Inequality operator for MapEdges.
        /// </summary>
        /// <param name="a">First MapEdge to compare.</param>
        /// <param name="b">Second MapEdge to compare.</param>
        /// <returns>True if:<list type="bullet">The MapEdges are not equal.</list>False if:<list type="bullet">The MapEdges are equal.</list></returns>
        public static bool operator !=(MapEdge a, MapEdge b) {
            return !(a == b);
        }

        /// <summary>
        /// Compare this MapEdge to another MapEdge. MapEdges are not directed to a flipped From/To is still equal.
        /// </summary>
        /// <param name="other">MapEdge to compare to.</param>
        /// <returns>True if:<list type="bullet"><paramref name="other"/> is equal to this MapEdge.</list>False if:<list type="bullet"><paramref name="other"/> is not equal to this object.</list></returns>
        public bool Equals(MapEdge other) {
            if (other == null) return false;

            return this == other;
        }

        /// <summary>
        /// Check if <paramref name="obj"/> is equal to this MapEdge. If <paramref name="obj"/> is a MapEdge, uses <see cref="Equals(MapEdge)"/>.
        /// </summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns>True if:<list type="bullet"><paramref name="obj"/> is equal to this MapEdge.</list>False if:<list type="bullet"><paramref name="obj"/> is not equal to this object.</list></returns>
        public override bool Equals(object obj) {
            if (obj == null) return false;

            MapEdge other = obj as MapEdge;
            if (other == null) return false;

            return Equals(other);

        }

        /// <summary>
        /// Get the hashcode for the MapEdge.
        /// </summary>
        /// <returns>Hash code for the object.</returns>
        public override int GetHashCode() {
            return HashCode.Combine(from, to) + HashCode.Combine(to, from);
        }

        // ======================== ^^^ Interface Implementation ^^^ ========================

        /// <summary>
        /// Connections for the from node along this edge.
        /// </summary>
        private HashSet<string> fromConnections = new HashSet<string>();

        /// <summary>
        /// Connections for the to node along this edge.
        /// </summary>
        private HashSet<string> toConnections = new HashSet<string>();

        /// <summary>
        /// Change the connections along this edge for <paramref name="node"/> to <paramref name="connections"/>.
        /// </summary>
        /// <param name="node">Node to change the connections for.</param>
        /// <param name="connections">Connections to change to.</param>
        /// <returns>True if:<list type="bullet"><paramref name="connections"/> differs from the current connections for <paramref name="node"/>.</list>False if:<list type="bullet"><item>The current connections match <paramref name="connections"/>.</item> <item><paramref name="node"/> is null or not along this edge.</item></list></returns>
        public bool ChangeConnectionsFor(MapNode node, HashSet<string> connections) {
            bool dirty = false;

            if (node == from) {
                if (!connections.SetEquals(fromConnections)) dirty = true;
                fromConnections = new HashSet<string>(connections);
            }
            if (node == to) {
                if (!connections.SetEquals(toConnections)) dirty = true;
                toConnections = new HashSet<string>(connections);
            }

            return dirty;
        }

        /// <summary>
        /// Get the connections associated with <paramref name="node"/>.
        /// </summary>
        /// <param name="node">Node to get the connections for.</param>
        /// <returns><list type="bullet"><item>Connections associated with <paramref name="node"/>.</item><item>Null if <paramref name="node"/> is null or not along this edge.</item></list></returns>
        public HashSet<string> GetConnectionsFor(MapNode node) {
            if (node == from) return fromConnections;
            if (node == to) return toConnections;

            return null;
        }

        /// <summary>
        /// Get the connections asscociated with the node that is not <paramref name="node"/>.
        /// </summary>
        /// <param name="node">Node the connections should not be gotten for.</param>
        /// <returns><list type="bullet"><item>Connections associated with the node that is not <paramref name="node"/>.</item><item>Null if <paramref name="node"/> is null or not along this edge.</item></list></returns>
        public HashSet<string> GetConnectionsForOther(MapNode node) {
            if (node == from) return toConnections;
            if (node == to) return fromConnections;

            return null;
        }
    }
}
