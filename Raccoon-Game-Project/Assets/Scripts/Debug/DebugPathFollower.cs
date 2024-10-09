// using System.Collections;
// using System.Collections.Generic;
// using Unity.Mathematics;
// using UnityEngine;

// public class DebugPathFollower : MonoBehaviour
// {
//     Path path;
//     int movementNodeIndex;
//     bool isForwards;
//     [SerializeField] Sprite dotSymbol; //indicator dots that show where the object is gonna go. Optional.
//     enum EndNodeBehaviour {Stop, Reverse, Warp}
//     [SerializeField] EndNodeBehaviour endNodeBehaviour;
//     // Start is called before the first frame update
//     void Start()
//     {
//         if (!TryGetComponent(out path))
//         {
//             enabled = false;
//             print("Need Path!");
//         }
//         movementNodeIndex = 0;
//         transform.position = path.nodes[movementNodeIndex].position;
//         isForwards = true;
//         if (dotSymbol)
//         {
//             GenerateDots();
//         }

//     }

//     // Update is called once per frame
//     //TODO: fix indexing errors on reverse.
//     void Update()
//     {
//         if (movementNodeIndex == path.nodes.Length - 1)
//         {
//             switch(endNodeBehaviour)
//             {
//                 case EndNodeBehaviour.Stop:
//                     return;
//                 case EndNodeBehaviour.Reverse:
//                     isForwards ^= true;
//                     movementNodeIndex--;
//                     break;
//                 case EndNodeBehaviour.Warp:
//                     transform.position = path.nodes[0].position;
//                     movementNodeIndex = 0;
//                     break;
//             }
//         }
//         print(movementNodeIndex + getDir());
//         if ((Vector2)transform.position == path.nodes[movementNodeIndex + getDir()].position)
//         {
//             movementNodeIndex += getDir();
//             return;
//         }
//         transform.position = Vector2.MoveTowards(
//             transform.position,
//             path.nodes[movementNodeIndex + getDir()].position,
//             path.nodes[movementNodeIndex].speed * Time.deltaTime
//             );
//     }

//     //create sprites that 
//     void GenerateDots()
//     {
//         for (int dotNodeIndex = 0; dotNodeIndex < path.nodes.Length - 1; dotNodeIndex++)
//         {
//             for (
//                 Vector2 dotPosition = path.nodes[dotNodeIndex].position; //position at node we want to start at
//                 dotPosition != path.nodes[dotNodeIndex + 1].position; //condition to loop: our marker position hasnt reached 
//                 dotPosition = Vector2.MoveTowards(dotPosition, path.nodes[dotNodeIndex + 1].position, 1)) //inch towards the other node 1 unit
//             {
//                 //create marker at position.
//                 GameObject markerObject = new("marker", new System.Type[] { typeof(SpriteRenderer) });
//                 markerObject.transform.position = dotPosition;
//                 markerObject.GetComponent<SpriteRenderer>().sprite = dotSymbol;
//             }
//         }

//     }
//     int getDir()
//     {
//         return isForwards?1:-1;
//     }
// }
