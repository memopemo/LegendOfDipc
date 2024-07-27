using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DemonBuffs
{
    public enum DemonBuff { Inactive,
    Attack0,    Regen0,     Speed0, Defense0,   Poision0,   Target0,    Loot0, 
    Swing0,     Fasting0,   Stun0,  Balance,    Flame0,     Puncture0,  Vacuum,
    Vault,      Teleport,   Astir,  Punch0,     Reflect,    Cloak,      Orbiter,
    Attack1,    Regen1,     Speed1, Defense1,   Trap,       Zip,        Health0,
    Swing1,     Focus,      Miss,   Resilient,  Poision1,   Target1,    Loot1,
    Swipe,      Fasting1,   Stun1,  Punch1,     Flame1,     Puncture1,  Health1,
    TripleHit,  Regen2,     Moment, Defense2,   Docile,     Distract,   Plunder,
    }
    [NonSerialized] public static DemonBuff[] demonBuffs = new DemonBuff[NUM_OF_BUFFS];
    [NonSerialized] public static float[] buffTimers = new float[NUM_OF_BUFFS];
    const int NUM_OF_BUFFS = 3;

    //ONLY THE PLAYER MAY CALL THIS!
    public static void Update()
    {
        for (int i = 0; i < NUM_OF_BUFFS-1; i++)
        {
            Timer.DecrementTimer(ref buffTimers[i]);
            if(buffTimers[i] <= 0)
            {
                demonBuffs[i] = DemonBuff.Inactive;
                buffTimers[i] = 0;
            }
        }
    }
    public static bool HasBuff(DemonBuff wantedBuff)
    {
        for (int i = 0; i < NUM_OF_BUFFS-1; i++)
        {
            if (wantedBuff ==  demonBuffs[i]) return true;
        }
        return false;
    }
}
