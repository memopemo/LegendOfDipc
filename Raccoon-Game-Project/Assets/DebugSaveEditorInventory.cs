using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugSaveEditorInventory : MonoBehaviour
{
    void Start()
    {
        int index = GameObjectParser.GetIndexFromName(gameObject);
        GetComponent<TMP_InputField>().text = SaveManager.GetSave().InventoryConsumableType[index].ToString();
        transform.Find("InputField (TMP)_1").GetComponent<TMP_InputField>().text = SaveManager.GetSave().InventoryConsumableCount[index].ToString();
    }
    public void SetItemType(string n)
    {
        SaveManager.GetSave().InventoryConsumableType[GameObjectParser.GetIndexFromName(gameObject)] = int.Parse(n);
    }

    public void SetItemCount(string n)
    {
        SaveManager.GetSave().InventoryConsumableCount[GameObjectParser.GetIndexFromName(gameObject)] = int.Parse(n);
    }
}
