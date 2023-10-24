
using System;
using System.Numerics;

[Serializable]
public class PresetTileData : IPresetTileData {
    public string Name { get; set; }
    public Vector3 Coordinate { get; set; }
    public int Rotation { get; set; }
}
