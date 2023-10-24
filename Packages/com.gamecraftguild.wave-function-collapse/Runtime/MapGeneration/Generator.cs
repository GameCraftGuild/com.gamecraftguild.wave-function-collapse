using System.Collections.Generic;

public class Generator {

    /// <summary>
    /// Generate the map.
    /// </summary>
    /// <param name="map">Map being generated.</param>
    public static void GenerateMap (IMap map) {
        map.CreateNodes();

        AssignPresetTiles (map);

        MapNode next = map.GetNextNodeToCollapse();
        while (next != null) {
            next.Collapse();

            next = map.GetNextNodeToCollapse();
        }
    }

    /// <summary>
    /// Force set any preset tiles on the map.
    /// </summary>
    /// <param name="map">Map being generated.</param>
    private static void AssignPresetTiles (IMap map) {
        MapNode temp;
        foreach (IPresetTile preset in map.PresetTiles) {
            if (preset != null) {
                temp = map.GetNodeAt(preset.Coordinate);
                if (temp != null) temp.ForceSet(preset);
            }
        }
    }

}
