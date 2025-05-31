using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSaveClear : MonoBehaviour
{
    #if DEBUG
    public void ClearSave()
    {
        SaveManager.ResetSave();
        Randomizer.StartRando(0);
    }
    #endif
}
