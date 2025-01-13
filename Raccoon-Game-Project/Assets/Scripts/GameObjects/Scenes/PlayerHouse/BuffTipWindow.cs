using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuffTipWindow : MonoBehaviour
{
    TMP_Text title;
    TMP_Text description;
    string[][] csv;


    // Start is called before the first frame update
    void Start()
    {
        csv = CSVReader.GetCSV("BuffDescriptions");
        title = GetComponentInChildren<TMP_Text>();
        description = title.transform.GetChild(0).GetComponent<TMP_Text>();
    }

    public void SetSelectedBuff(int selectedIndex)
    {
        if (GameObject.Find("Button_" + selectedIndex.ToString()).GetComponent<BuffSelector>().state == BuffSelector.State.Inactive)
        {
            title.text = "???";
            description.text = "???";
            return;
        }
        title.text = csv[selectedIndex][0];
        description.text = csv[selectedIndex][1];
    }
}
