using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    [SerializedDictionary("GameObject to Spawn", "Weight")]
    public SerializedDictionary<GameObject, int> LootTable;
    List<GameObject> expandedLootList;
    public List<GameObject> forcedLoot;
    public int averageLootRolls; // may be +-1
    // Start is called before the first frame update
    void Start()
    {
        //populate expanded loot list
        expandedLootList = new List<GameObject>();

        //for each entry in dictionary
        foreach (var i in LootTable)
        {
            //add {weight} amount of {loot} to the expanded table.
            for (int j = 0; j < i.Value; j++)
            {
                expandedLootList.Add(i.Key);
            }
        }
        /*foreach (var item in expandedLootList)
        {
            print(item);
        }*/
    }

    // Update is called once per frame
    void Update()
    {

    }

    /* Loot and weight distribution think-through:
     * Start() will generate an ExpandedLootList with:
     * Each gameobject is put on the ExpandedLootList <weight> number of times.
     * this means that when the random number generates it, higher weights will be rolled more often.
     * You can think of this as a raffle giveaway where you can cheat and put a name in multiple times.
     * Then, when it comes time to spawn the loot, it will roll averageLootRolls(+ or - 1).
     * Each roll, it will pick a random entry within ExpandedLootList and then drop that loot.
     * Because it picks random, we do not need to randomize the list itself.
     */

    public void SpawnLoot()
    {

        // Spawn forced loot.
        foreach (var i in forcedLoot)
        {
            Instantiate(i, transform.position, Quaternion.identity);
        }

        // early exit/test if there is no general loot to roll.
        if (expandedLootList.Count == 0 || averageLootRolls <= 0) return;
        int setLootRoll = averageLootRolls + Random.Range(averageLootRolls - 1, averageLootRolls + 1); //+- 1
        if (setLootRoll <= 0) setLootRoll = 1; //prevent 0 and negative numbers.
        for (int i = 1; i <= setLootRoll; i++)
        {

            Instantiate(
                expandedLootList[Random.Range(0, expandedLootList.Count - 1)],
                transform.position,
                Quaternion.identity
                );
        }

    }
}