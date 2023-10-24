using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{

    private HexMap map;

    // Start is called before the first frame update
    void Start()
    {
        map = new HexMap("testMap", new TileFactory(), new JsonLoader("Json/Maps/", "Json/Tiles/PresetTiles/", "Json/Tiles/TileLists/", "Json/Tiles/TileData/", "Json/Tiles/TileConnections/"), HexNodeGenerationFunctions.nodeGenerationMapping);
        try {
            Generator.GenerateMap(map);
        } catch (MapGenerationException e) {
            Debug.LogException(e);
        }

        GameObject go;
        foreach (MapNode node in map.AllNodes) {
            if (node == null || node.SetTile == null) continue;
            go = GameObject.Instantiate(Resources.Load("Prefabs/Tiles/" + node.SetTile.Name), new Vector3(), Quaternion.identity) as GameObject;
            go.transform.position = new Vector3(Mathf.Pow(3, 0.5f) * node.Coordinate.X + (Mathf.Pow(3, 0.5f) / 2) * node.Coordinate.Y, 0, -1.5f * node.Coordinate.Y);
            go.transform.eulerAngles = new Vector3(go.transform.eulerAngles.x, -60 * node.SetTile.Rotation, go.transform.eulerAngles.z);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
