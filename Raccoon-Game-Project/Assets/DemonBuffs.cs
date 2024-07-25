using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBuffs : MonoBehaviour
{
    public enum DemonBuff { Inactive,
    Attack0,  Attack1,  Swing0,    Swing1,    Vault,    Swipe,    TripleHit, 
    Regen0,   Regen1,   Regen2,    Fasting0,  Fasting1, Teleport, Focus, 
    Speed0,   Speed1,   Stun0,     Stun1,     Astir,    Miss,     Moment,
    Defense0, Defense1, Defense2,  Balance,   Punch0,   Punch1,   Resilient,
    Poision0, Poision1, Flame0,    Flame1,    Reflect,  Trap,     Docile,
    Target0, Target1,   Puncture0, Puncture1, Cloak,    Zip,      Distract,
    Loot0,   Loot1,     Vacuum,    Orbiter,   Health,   Plunder,
    }
    [NonSerialized] public DemonBuff[] demonBuffs;
    [NonSerialized] public float[] buffTimers;
    const int NUM_OF_BUFFS = 3;
    // Start is called before the first frame update
    void Start()
    {
        demonBuffs = new DemonBuff[NUM_OF_BUFFS];
        buffTimers = new float[NUM_OF_BUFFS];
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < NUM_OF_BUFFS-1; i++)
        {
            Timer.DecrementTimer(ref buffTimers[i]);
            if(buffTimers[i] <= 0)
            {
                demonBuffs[i] = DemonBuff.Inactive;
            }
        }
    }
    public bool HasBuff(DemonBuff wantedBuff)
    {
        for (int i = 0; i < NUM_OF_BUFFS-1; i++)
        {
            if (wantedBuff ==  demonBuffs[i]) return true;
        }
        return false;
    }
}
