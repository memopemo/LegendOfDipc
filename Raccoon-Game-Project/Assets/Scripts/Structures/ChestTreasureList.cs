using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ChestTreasureList", menuName = "Chest List", order = 2)]
public class ChestTreasureList : ScriptableObject
{
    public enum GiveWhat { Nothing, KeySelectableItem, KeyUnselectableItem, ConsumableItem, Money, Furniture, Sword, Shield, Boomerang, Armor, HeartContainer }
    [System.Serializable]
    public class ChestTreasure
    {
        public GiveWhat giveWhat;
        public int indexOrQuantity; //used as the quantity or index into the array of the thing to set to true.
        public Sprite spriteInsideChest;
    }
    public ChestTreasure[] chestTreasure = new ChestTreasure[1];
}
