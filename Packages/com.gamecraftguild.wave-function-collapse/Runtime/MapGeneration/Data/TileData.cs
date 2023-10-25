using System;
using System.Collections.Generic;

namespace GameCraftGuild.WaveFunctionCollapse {

    [Serializable]
    public class TileData : ITileData {
        public string Name { get; set; }
        public HashSet<string> Tags { get; set; }
        public string[] Connections { get; set; }
        public int Probability { get; set; }
        public Dictionary<string, int> ProbabilityModifiers { get; set; }
    }

}