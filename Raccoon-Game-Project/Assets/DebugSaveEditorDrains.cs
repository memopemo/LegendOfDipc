using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSaveEditorDrains : MonoBehaviour
{
    public void SetDrain(bool b)
    {
        SaveManager.GetSave().UncloggedDrainPipes[GameObjectParser.GetIndexFromName(gameObject)] = b;
    }
}
