using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSaveEditorEquipment : MonoBehaviour
{
    public void SetSword(string n)
    {
        SaveManager.GetSave().CurrentSword = int.Parse(n);
    }
    public void SetShield(string n)
    {
        SaveManager.GetSave().CurrentShield = int.Parse(n);
    }
    public void SetArmor(string n)
    {
        SaveManager.GetSave().CurrentArmor = int.Parse(n);
    }
    public void SetBoomerang(string n)
    {
        SaveManager.GetSave().CurrentBoomerang = int.Parse(n);
    }
}
