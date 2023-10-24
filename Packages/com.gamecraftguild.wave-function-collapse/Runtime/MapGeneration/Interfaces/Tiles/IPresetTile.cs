using System.Numerics;

public interface IPresetTile : ITile
{

    /// <summary>
    /// Coordinate for this preset tile.
    /// </summary>
    public Vector3 Coordinate { get; }

}
