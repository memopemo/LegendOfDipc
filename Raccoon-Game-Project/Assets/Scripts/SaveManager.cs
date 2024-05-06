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

    public static SaveFile GetSave()
    {
        if (saveManager == null)
        {
            Load();
        }

        return saveManager;
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
                saveManager = new SaveFile();
            }

        }
        else
        {
            saveManager = new SaveFile();
        }
    }
    //Returns: True if item was sucessfully decremented
    //  False if The index points to an invalid ID type
    public static bool UseUpConsumableItem(int index)
    {
        SaveFile sf = GetSave();
        if(sf.InventoryConsumableType[index] <= 0 || sf.InventoryConsumableCount[index] <= 0)
        {
            sf.InventoryConsumableType[index] = 0;
            sf.InventoryConsumableCount[index] = 0;
            return false;
        }
        sf.InventoryConsumableCount[index]--;
        return true;

    }
    public static void DebugMaxOut()
    {
        SaveFile sf = GetSave();
        SetArrayToTrue(ref sf.HeartContainersCollected);
        sf.Money = 999;
        sf.ToiletPaperRolls = 99;
        sf.ComputerBombs = 99;
        sf.ComputerParts = 99;
        sf.InventoryConsumableCount[0] = 10;
        sf.InventoryConsumableType[0] = 1;
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
        SetArrayToTrue(ref sf.OverworldTreasure);
        SetArrayToTrue(ref sf.GenericFlags);

    }
    static void SetArrayToTrue(ref bool[] a)
    {
        for (int i = 0; i < a.Length; i++)
        {
            a[i] = true;
        }
    }
}
