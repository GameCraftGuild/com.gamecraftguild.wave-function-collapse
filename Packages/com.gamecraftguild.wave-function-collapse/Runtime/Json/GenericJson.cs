using Newtonsoft.Json;
using UnityEngine;

public class GenericJson {

    /// <summary>
    /// Load a json object from a file
    /// </summary>
    /// <typeparam name="T">Type of the object to load.</typeparam>
    /// <param name="path">Path to the object to load. Should be inside the Resources folder.</param>
    /// <returns>Loaded object.</returns>
    public static T LoadFromFile<T>(string path) {
        TextAsset file = Resources.Load(path) as TextAsset;
        string json = file.text;

        T obj = JsonConvert.DeserializeObject<T>(json);

        return obj;
    }

}