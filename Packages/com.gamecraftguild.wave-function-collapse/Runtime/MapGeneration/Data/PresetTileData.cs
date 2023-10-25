using System;
using System.Numerics;

namespace GameCraftGuild.WaveFunctionCollapse {

    [Serializable]
    public class PresetTileData : IPresetTileData {
        public string Name { get; set; }
        public Vector3 Coordinate { get; set; }
        public int Rotation { get; set; }
    }

}