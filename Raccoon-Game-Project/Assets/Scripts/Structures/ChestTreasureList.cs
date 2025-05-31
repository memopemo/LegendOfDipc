using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "ChestTreasureList", menuName = "Chest List", order = 2)]
public class ChestTreasureList : ScriptableObject
{
    public Treasure[] chestTreasure = new Treasure[1];
}

[System.Serializable]
public class Treasure
{
    public enum GiveWhat { Nothing, KeySelectableItem, KeyUnselectableItem, ConsumableItem, Money, Furniture, Sword, Shield, Boomerang, Armor, HeartContainer, Key, SkeletonKey, BossKey, ToiletPaper, StoryKey, Pendant}
    public GiveWhat giveWhat;
    public int indexOrQuantity; //used as the quantity or index into the array of the thing to set to true.
    public Sprite spriteInsideChest;
    public void OnRecieved()
    {
        SaveFile save = SaveManager.GetSave();

        switch (giveWhat)
        {
            case GiveWhat.Nothing:
                break;

            case GiveWhat.KeySelectableItem:
                //index
                save.ObtainedKeyItems[indexOrQuantity] = true;
                SelectedItem.KeyItem = indexOrQuantity; //select new item as ease to find out what it does.
                break;

            case GiveWhat.KeyUnselectableItem:
                //index
                save.ObtainedKeyUnselectableItems[indexOrQuantity] = true;
                break;

            case GiveWhat.ConsumableItem:
                //index
                SaveManager.AddConsumableItem(indexOrQuantity);
                break;

            case GiveWhat.Money:
                //quantity
                save.Money += indexOrQuantity;
                break;

            case GiveWhat.Furniture:
                //index
                save.HouseItems[indexOrQuantity] = true;
                break;

            case GiveWhat.Sword:
                //index
                save.Swords[indexOrQuantity] = true;
                save.CurrentSword = indexOrQuantity;
                break;

            case GiveWhat.Shield:
                //index
                save.Shields[indexOrQuantity] = true;
                save.CurrentShield = indexOrQuantity;
                break;

            case GiveWhat.Boomerang:
                // index
                save.Boomerangs[indexOrQuantity] = true;
                save.CurrentBoomerang = indexOrQuantity;
                break;

            case GiveWhat.Armor:
                //index
                save.Armors[indexOrQuantity] = true;
                save.CurrentArmor = indexOrQuantity;
                break;

            case GiveWhat.HeartContainer:
                //index
                save.HeartContainersCollected[indexOrQuantity] = true;
                PlayerHealth.Heal(2);
                break;

            case GiveWhat.Key:
                //index is split as follows:
                //  0-13: dungeon 1 keys, 
                // 14-27: dungeon 2 keys,
                // etc
                save.dungeons[indexOrQuantity / Dungeon.MAX_KEYS].KeyObtained[indexOrQuantity % Dungeon.MAX_KEYS] = true;
                break;

            case GiveWhat.SkeletonKey:
                // index
                save.dungeons[indexOrQuantity].SkeletonKeyObtained = true;
                break;

            case GiveWhat.BossKey:
                // index
                save.dungeons[indexOrQuantity].BossKeyObtained = true;
                break;
            case GiveWhat.ToiletPaper:
                save.ToiletPaperRolls += 1;
                break;
            case GiveWhat.StoryKey:
                save.DemonKeys[indexOrQuantity] = true;
                break;
            case GiveWhat.Pendant:
                save.Pendants[indexOrQuantity] = true;  
                break;
        }
    }
}
