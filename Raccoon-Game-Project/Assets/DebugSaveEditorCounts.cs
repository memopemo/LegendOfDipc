using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSaveEditorCounts : MonoBehaviour
{
    public void SetTimePlayed(string n)
    {
        SaveManager.GetSave().TimePlayed = int.Parse(n);
    }
    public void SetMoney(string n)
    {
        SaveManager.GetSave().Money = int.Parse(n);
    }
    public void SetSavePoint(string n)
    {
        SaveManager.GetSave().SavePoint = int.Parse(n);
    }
    public void SetToiletPaper(string n)
    {
        SaveManager.GetSave().ToiletPaperRolls = int.Parse(n);
    }
    public void SetComputerParts(string n)
    {
        SaveManager.GetSave().ComputerParts = int.Parse(n);
    }
    public void SetComputerBombs(string n)
    {
        SaveManager.GetSave().ComputerBombs = int.Parse(n);
    }
}
