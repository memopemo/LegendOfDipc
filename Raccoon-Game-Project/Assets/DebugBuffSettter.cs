using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBuffSettter : MonoBehaviour
{
    public void SetBuff(int buff)
    {
        DemonBuffs.demonBuffs[0] = (DemonBuffs.DemonBuff)buff;
        DemonBuffs.buffTimers[0] = 65;
    }
}
