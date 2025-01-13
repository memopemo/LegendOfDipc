using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DemonBuffs
{
    public enum DemonBuff
    {
        Inactive,
        Attack0, Regen0, Speed0, Defense0, Poision0, Target0, Loot0,
        Swing0, Fasting0, Stun0, Balance, Flame0, Puncture0, Vacuum,
        Phase, BackAttack, Astir, Punch0, Reflect, Cloak, Plunder0,
        Attack1, Regen1, Speed1, Defense1, Trap, Zip, Health0,
        Swing1, Focus, Miss, Resilient, Poision1, Target1, Loot1,
        Swipe, Fasting1, Stun1, Punch1, Flame1, Puncture1, Health1,
        TripleHit, Regen2, Cut, Defense2, Docile, Vision, Plunder1,
    }
    [NonSerialized] public static DemonBuff[] demonBuffs = new DemonBuff[NUM_OF_BUFFS];
    [NonSerialized] public static float[] buffTimers = new float[NUM_OF_BUFFS];
    const int NUM_OF_BUFFS = 3;

    //TODO: Fill this shit out
    public static Dictionary<DemonBuff, float> times = new()
    {
        {DemonBuff.Inactive, "YouMessedUp".GetHashCode()},
        {DemonBuff.Attack0, 600},
    };

    //ONLY THE PLAYER MAY CALL THIS!
    public static void Update()
    {
        for (int i = 0; i < NUM_OF_BUFFS; i++)
        {
            if (demonBuffs[i] == DemonBuff.Inactive) continue;
            Timer.DecrementTimer(ref buffTimers[i]);
            if (buffTimers[i] <= 0)
            {
                demonBuffs[i] = DemonBuff.Inactive;
                buffTimers[i] = 0;
            }
        }
    }
    public static bool HasBuff(DemonBuff wantedBuff)
    {
        for (int i = 0; i < NUM_OF_BUFFS; i++)
        {
            if (wantedBuff == demonBuffs[i]) return true;
        }
        return false;
    }
    public static void AddBuff(DemonBuff demonBuff)
    {
        for (int i = 0; i < NUM_OF_BUFFS; i++)
        {
            if (demonBuffs[i] == DemonBuff.Inactive)
            {
                demonBuffs[i] = demonBuff;
                buffTimers[i] = 600;
                return;
            }
        }
    }
    public static void ClearBuffs()
    {
        for (int i = 0; i < NUM_OF_BUFFS; i++)
        {
            demonBuffs[i] = DemonBuff.Inactive;
            buffTimers[i] = 0;
        }
    }
    public static bool IsFull()
    {
        return demonBuffs.All((x) => x != DemonBuff.Inactive);
    }
}
