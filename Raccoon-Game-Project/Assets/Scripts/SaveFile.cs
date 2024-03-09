using System;


public class SaveFile
{
    public string logo = "DIPC";
    // General
    public int TimePlayed;
    public int Money;
    public int SavePoint;
    public bool[] HeartContainersCollected = new bool[13];
    public int ToiletPaperRolls;
    public int ComputerParts;
    public int ComputerBombs;
    public bool[] SecretsFound = new bool[64];
    public bool[] ObtainedKeyItems = new bool[18];
    public bool[] ObtainedKeyUnselectableItems = new bool[6];
    public bool[] UncloggedDrainPipes = new bool[6];

    // Inventory
    public int[] InventoryConsumableType = new int[24];
    public int[] InventoryConsumableCount = new int[24];

    // Saved Equipment
    public int CurrentSword;
    public int CurrentShield;
    public int CurrentArmor;
    public int CurrentBoomerang;

    // House
    public int[] HouseLayout = new int[24];

    // Collected
    public bool[] HouseItems = new bool[64];
    public bool[] Swords = new bool[12];
    public bool[] Shields = new bool[4];
    public bool[] Armors = new bool[4];
    public bool[] Boomerangs = new bool[4];
    public bool[] Pendants = new bool[7];
    public bool[] DemonKeys = new bool[7];


    // Dungeons
    public Dungeon[] dungeons = new Dungeon[14];

    //etc flags
    public bool[] OverworldTreasure = new bool[16];
    public bool[] GenericFlags = new bool[10];
}

[Serializable]
public class Dungeon
{
    public bool BossDefeated;
    public bool MiniBossDefeated;
    public bool BossKeyObtained;
    public bool SkeletonKeyObtained;
    public bool[] KeyObtained = new bool[16];
    public bool[] ChestOpened = new bool[16];
}
