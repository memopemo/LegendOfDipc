using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPanel : MonoBehaviour
{
    SaveFile saveFile;
    public GameObject[] sectionsToggle;
    GameObject currentSection;
    void Start()
    {
        saveFile = SaveManager.GetSave();
        currentSection = sectionsToggle[0];
        currentSection.SetActive(true);
    }
    public void SetArmor(string i)
    {
        saveFile.CurrentArmor = int.Parse(i);
    }
    public void SetSword(string i)
    {
        saveFile.CurrentSword = int.Parse(i);
    }
    public void SetShield(string i)
    {
        saveFile.CurrentShield = int.Parse(i);
    }
    public void SetBoomerang(string i)
    {
        saveFile.CurrentBoomerang = int.Parse(i);
    }
    public void ToggleSection(int i)
    {
        currentSection.SetActive(false);
        currentSection = sectionsToggle[i];
        currentSection.SetActive(true);

    }
}
