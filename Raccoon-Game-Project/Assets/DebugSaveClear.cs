using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSaveClear : MonoBehaviour
{
    public void ClearSave()
    {
        SaveManager.ResetSave();
    }
}
