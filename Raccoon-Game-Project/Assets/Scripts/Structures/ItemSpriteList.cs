using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSpriteList", menuName = "Item Sprite List", order = 3)]
public class ItemSpriteList : ScriptableObject
{
    public Sprite[] consumableItems = new Sprite[19];
    public Sprite[] keyItems = new Sprite[18];
    public Sprite[] swords = new Sprite[12];
    public Sprite[] shields = new Sprite[4];
    public Sprite[] armors = new Sprite[4];
    public Sprite[] boomerangs = new Sprite[4];
}
