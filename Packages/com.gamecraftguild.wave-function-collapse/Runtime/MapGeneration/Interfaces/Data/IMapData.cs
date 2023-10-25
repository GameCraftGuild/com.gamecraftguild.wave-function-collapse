namespace GameCraftGuild.WaveFunctionCollapse {

    public interface IMapData {
        /// <summary>
        /// File name for the tile list.
        /// </summary>
        public string TileListName { get; set; }

        /// <summary>
        /// File name for the tile connections.
        /// </summary>
        public string TileConnectionsName { get; set; }

        /// <summary>
        /// File list for the preset tiles.
        /// </summary>
        public string PresetTilesName { get; set; }

    }

}