using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSaveEditorSecrets : MonoBehaviour
{
    public void SetSecret(bool b)
    {
        SaveManager.GetSave().SecretsFound[GameObjectParser.GetIndexFromName(gameObject)] = b;
    }
}
