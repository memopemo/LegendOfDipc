using System.Collections.Generic;
using UnityEngine;

public static class Randomizer
{
    public static int[] randomizedChestIndexMapping = new int[SaveFile.NUM_CHESTS];
    public static bool isRandoActive = false;
    //TODO: route checking, or maybe like a wave function collapse algorithm?
    //REQUIRED: make the rest of the fucking game
    public static void StartRando(int seed)
    {
        Random.InitState(seed);
        //start at 1 so we dont randomize index 0 (which is a failsafe "nothing" loot)
        for (int i = 1; i < randomizedChestIndexMapping.Length; i++)
        {
            randomizedChestIndexMapping[i] = Random.Range(i, randomizedChestIndexMapping.Length);
        }
        isRandoActive = true;
    }

}
