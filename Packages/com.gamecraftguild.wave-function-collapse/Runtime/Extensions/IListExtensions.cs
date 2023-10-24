using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

namespace GameCraftGuild.UnityExtensions.IList {

    /// <summary>
    /// Extensions for IList
    /// </summary>
    public static class IListExtensions {

        /// <summary>
        /// Randomly select an object from <paramref name="list" />.
        /// </summary>
        /// <param name="list">List of objects to choose from.</param>
        /// <typeparam name="T">Type of object in <paramref name="list" />.</typeparam>
        /// <returns>Selected object from <paramref name="list" /> or default(<typeparamref name="T" />) if <paramref name="list" /> is null or empty.</returns>
        public static T GetRandomFromList<T> (this IList<T> list) {
            if (list == null || list.Count == 0) return default(T);
            if (list.Count == 1) return list[0];

            return list[Random.Range(0, list.Count)];
        }

        /// <summary>
        /// Randomly select an object from <paramref name="objects" /> using the weights in <paramref name="weights" /> given for each object. An object with a nonpositive weight (weight &lt;= 0) will not be selected.
        /// </summary>
        /// <remarks>
        /// The type for <paramref name="weights" /> should be changed from "int" to "INumerics" once Unity supports .NET 7 and generic math.
        /// </remarks>
        /// <param name="objects">List of objects of type :<typeparamref name="T" />.</param>
        /// <param name="weights">Weights for each object.</param>
        /// <typeparam name="T">Type of objects in <paramref name="objects" />.</typeparam>
        /// <returns>Selected object from <paramref name="objects" /> or default(<typeparamref name="T" />) if <paramref name="objects" /> or <paramref name="weights" /> is null or empty, the list lengths do not match, or all objects have nonpositive weight (weight &lt;= 0).</returns>
        public static T GetRandomFromWeightedList<T> (this IList<T> objects, IList<int> weights) {
            if (objects == null || weights == null || objects.Count == 0 || weights.Count == 0 || objects.Count != weights.Count) return default(T);
            if (objects.Count == 1) return (weights[0] > 0 ? objects[0] : default(T));

            int totalWeight = 0;
            for (int index = 0; index < weights.Count; index++) {
                if (weights[index] <= 0) continue;
                totalWeight += weights[index];
            }

            if (totalWeight == 0) return default(T);

            int resultValue = Random.Range(0, totalWeight);
            int resultIndex = 0;

            while (resultIndex < objects.Count && resultValue >= weights[resultIndex]) {
                if (weights[resultIndex] <= 0) continue;
                resultValue -= weights[resultIndex];
                resultIndex++;
            }

            if (resultIndex >= objects.Count) return default(T);
            return objects[resultIndex];
        }
    }
}
