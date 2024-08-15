using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FurnitureList", menuName = "Furniture List", order = 3)]
public class FurnitureList : ScriptableObject
{
    public GameObject[] furniture;
}
