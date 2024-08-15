using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSaveEditorKeyItems : MonoBehaviour
{
    public void SetUsableKeyItem(bool b)
    {
        SaveManager.GetSave().ObtainedKeyItems[GameObjectParser.GetIndexFromName(gameObject)] = b;
    }
    public void SetAbilityKeyItem(bool b)
    {
        SaveManager.GetSave().ObtainedKeyUnselectableItems[GameObjectParser.GetIndexFromName(gameObject)] = b;
    }
}
