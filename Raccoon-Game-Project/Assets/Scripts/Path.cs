// using UnityEngine;
// using UnityEditor;
// using System;
// using System.Collections.Generic;
// using Unity.Mathematics;

// [Serializable]
// public class Path : MonoBehaviour
// {
//     public Node[] nodes;
//     [Serializable] public class Node
//     {
//         public Vector2 position;
//         public float speed;
//     }
// }

// [CustomEditor(typeof(Path))]
// public class PathEditor: Editor
// {
//     // Custom in-scene UI for when ExampleScript
//     // component is selected.
//     public void OnSceneGUI()
//     {
//         var color = Color.white;
//         Path path = target as Path;
//         Handles.color =  color;
//         Handles.DrawDottedLines(ConvertToPointList(path.nodes), 1);
//         foreach (var node in path.nodes)
//         {
//             node.position = Handles.PositionHandle(node.position, quaternion.identity);
//             Handles.Label(node.position, node.speed.ToString());
//         }
//     }
//     public Vector3[] ConvertToPointList(Path.Node[] vectors)
//     {
//         List<Vector3> results = new();
        
//         for (int i = 0; i < vectors.Length; i++)
//         {
//             results.Add(vectors[i].position);
//             if (i != 0 && i != vectors.GetUpperBound(0)) // add 2 of them if they are not first or last.
//             {
//                 results.Add(vectors[i].position);
//             }
//         }
//         return results.ToArray();
//     }
// }