using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ItemList", menuName = "Item List", order = 2)]
public class ItemList : ScriptableObject
{
    public GameObject[] ConsumableItems = new GameObject[12];
    public GameObject[] UsableKeyItems = new GameObject[18];
}
