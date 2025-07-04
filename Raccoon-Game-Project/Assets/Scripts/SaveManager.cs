﻿using System;
using System.IO;
using UnityEngine;

/* how it works
 * 
 * object: "hey id like the save file plz*
 * savemanager: "ok here you go"
 * or
 * savemanager: "oops it doesnt exist/ is invalid/havent loaded it yet"
 *  "lemme make a new one/ load it"
 *  "here ya go"
 * obj: "thx"
 * savemngr: "..."
 * obj: "..."
 * *starts passionately making out*
*/

public static class SaveManager
{
    private static SaveFile saveManager;
    public static string SaveFilePath = Application.persistentDataPath + "/SaveFile.json";
    public static float timeSinceLastSave;

    public static SaveFile GetSave()
    {
        if (saveManager == null)
        {
            Load();
        }

        return saveManager;
    }
    public static bool SaveFileExists()
    {
        return File.Exists(SaveFilePath);
    }

    public static void Save()
    {
        /* Do I really care that the save file is easily editable?
         * No.
         * If people want to cheat, then they can, and it should be atleast easy to do so.
         * If people want to try my challenge, they can too.
         * Despite like *anything* i could do, people are still gonna reverse engineer it and make a savefile editor online if i try.
         * So why bother?
         * Plus, it may get people more invested in coding and bring more people into coding games and stuff.
         * I like peeking into old games and new games, and I should continue that for other people who want to.
         * Its what made me, me.
         * 
         * But because its easily editable, i need to design my game so that it would be worth it to *not* cheat.
         * Make a sort of game that isnt so bad that you'd rather just beat it by editing the save file.
         * 
         * That may be easy?
         */
        File.WriteAllText(SaveFilePath, JsonUtility.ToJson(GetSave(), true));
        timeSinceLastSave = Time.realtimeSinceStartup;
    }
    static void Load()
    {
        if (File.Exists(SaveFilePath))
        {
            try
            {
                saveManager = JsonUtility.FromJson(File.ReadAllText(SaveFilePath), typeof(SaveFile)) as SaveFile;
            }
            catch (System.Exception)
            {
                Debug.Log("Save file not valid. Generating blank one");
                ResetSave();
            }

        }
        else
        {
            Debug.Log("Save file not found. Generating blank one");
            ResetSave();
        }
    }
    public static void ResetSave()
    {
        saveManager = new SaveFile();
        InitializeSave();
    }
    //Returns: True if item was sucessfully decremented
    //  False if The index points to an invalid ID type, or is 0.
    public static bool UseUpConsumableItem(int index)
    {
        SaveFile sf = GetSave();
        if (sf.InventoryConsumableType[index] <= 0 || sf.InventoryConsumableCount[index] <= 0)
        {
            sf.InventoryConsumableType[index] = 0;
            sf.InventoryConsumableCount[index] = 0;
            return false;
        }
        sf.InventoryConsumableCount[index]--;
        //if we have used our last item.
        if (sf.InventoryConsumableCount[index] == 0)
        {
            sf.InventoryConsumableType[index] = 0; //remove item type
        }
        return true;

    }
    public static bool CanUseConsumableItem(int index)
    {
        SaveFile sf = GetSave();
        if (sf.InventoryConsumableType[index] <= 0 || sf.InventoryConsumableCount[index] <= 0)
        {
            sf.InventoryConsumableType[index] = 0;
            sf.InventoryConsumableCount[index] = 0;
            return false;
        }
        return true;
    }

    public static void DecrementConsumableItem(int index)
    {
        SaveFile sf = GetSave();
        sf.InventoryConsumableCount[index]--;
    }

    //adds a single new item to the inventory
    public static bool AddConsumableItem(int type)
    {
        if (type <= 0 || type > 18)
        {
            Debug.LogError($"Adding Consumable Item {type} is not 1-18!");
            return false;
        }
        SaveFile sf = GetSave();
        //find either an empty slot or the same type.
        for (int i = 0; i < sf.InventoryConsumableCount.Length; i++)
        {
            if (sf.InventoryConsumableType[i] == type)
            {
                if (sf.InventoryConsumableCount[i] == GameDefinitions.MAX_ITEM_STACK)
                {
                    continue;
                }
                sf.InventoryConsumableCount[i]++;
                return true;
            }
            else if (sf.InventoryConsumableType[i] == 0)
            {
                sf.InventoryConsumableType[i] = type;
                sf.InventoryConsumableCount[i] = 1;
                return true;
            }
        }
        //item could not fit anywhere.
        return false;
    }

#if DEBUG
    public static void DebugMaxOut()
    {
        SaveFile sf = GetSave();
        SetArrayToTrue(ref sf.HeartContainersCollected);
        sf.Money = 999;
        sf.ToiletPaperRolls = 99;
        sf.ComputerBombs = 99;
        sf.ComputerParts = 99;
        for(int i = 0; i < sf.InventoryConsumableCount.Length; i++)
        {
            sf.InventoryConsumableType[i] = i < sf.InventoryConsumableCount.Length/2?12:(i%5+13);
            sf.InventoryConsumableCount[i] = 8;
        }
        for(int i = 0; i < sf.dungeons.Length; i++)
        {
            Dungeon dungeon = sf.dungeons[i];
            SetArrayToTrue(ref sf.dungeons[i].DoorsUnlocked);
            SetArrayToTrue(ref sf.dungeons[i].KeyObtained);
            dungeon.MiniBossDefeated = true;
            dungeon.BossDefeated = true;
            dungeon.SkeletonKeyObtained = true;
            dungeon.BossKeyObtained = true;
        }
        SetArrayToTrue(ref sf.HeartContainersCollected);
        SetArrayToTrue(ref sf.SecretsFound);
        SetArrayToTrue(ref sf.ObtainedKeyItems);
        SetArrayToTrue(ref sf.ObtainedKeyUnselectableItems);
        SetArrayToTrue(ref sf.UncloggedDrainPipes);
        SetArrayToTrue(ref sf.HouseItems);
        SetArrayToTrue(ref sf.Swords);
        SetArrayToTrue(ref sf.Shields);
        SetArrayToTrue(ref sf.Armors);
        SetArrayToTrue(ref sf.Boomerangs);
        SetArrayToTrue(ref sf.Pendants);
        SetArrayToTrue(ref sf.DemonKeys);
        SetArrayToTrue(ref sf.ChestOpened);
        SetArrayToTrue(ref sf.GenericFlags);
        sf.CurrentSword = 11;
        sf.CurrentShield = sf.CurrentBoomerang = sf.CurrentArmor = 3;
        PlayerHealth.Heal(100);

    }
#endif

    static void InitializeSave()
    {
        //Everything else is already set to 0, but missing items must be -1.
        SaveFile sf = GetSave();
        sf.CurrentSword = -1;
        sf.CurrentShield = -1;
        sf.CurrentBoomerang = -1;
        sf.CurrentArmor = -1;

    }

    static void SetArrayToTrue(ref bool[] a)
    {
        for (int i = 0; i < a.Length; i++)
        {
            a[i] = true;
        }
    }
}
