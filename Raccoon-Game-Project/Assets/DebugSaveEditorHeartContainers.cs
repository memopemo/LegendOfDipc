using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSaveEditorHeartContainers : MonoBehaviour
{
    public void SetHeartContainer(bool b)
    {
        SaveManager.GetSave().HeartContainersCollected[GameObjectParser.GetIndexFromName(gameObject)] = b;
    }
}
