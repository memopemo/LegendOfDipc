using System;


public class SaveFile
{
    public string logo = "DIPC";
    // General
    public int TimePlayed;
    public int Money;
    public int SavePoint;
    public float TimeOfDay;
    public int Days;
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
    public int[] HouseLayout = new int[128];

    // Collected
    public bool[] HouseItems = new bool[64];
    public bool[] Swords = new bool[12];
    public bool[] Shields = new bool[4];
    public bool[] Armors = new bool[4];
    public bool[] Boomerangs = new bool[4];
    public bool[] Pendants = new bool[7];
    public bool[] DemonKeys = new bool[7];


    // Dungeons
    public const int NUM_DUNGEONS = 14;
    public Dungeon[] dungeons = new Dungeon[NUM_DUNGEONS];
    

    //etc flags
    public const int NUM_CHESTS = 128;
    public bool[] ChestOpened = new bool[NUM_CHESTS];
    public bool[] GenericFlags = new bool[10];
}

[Serializable]
public class Dungeon
{
    public bool BossDefeated;
    public bool MiniBossDefeated;
    public bool BossKeyObtained;
    public bool SkeletonKeyObtained;
    public const int MAX_KEYS = 16;
    public const int MAX_DOORS = 32;
    public bool[] KeyObtained = new bool[16];
    public bool[] DoorsUnlocked = new bool[32];
}
