using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuffTipWindow : MonoBehaviour
{
    UILanguagedText title;
    UILanguagedText description;


    // Start is called before the first frame update
    void Awake()
    {
        title = GetComponentInChildren<UILanguagedText>();
        description = title.transform.GetChild(0).GetComponent<UILanguagedText>();
    }

    public void SetSelectedBuff(int selectedIndex)
    {
        BuffSelector.State state = GameObject.Find("Buff_" + selectedIndex.ToString()).GetComponent<BuffSelector>().state;
        title.SetPath(state == BuffSelector.State.Inactive ? "buffText.blank.name" : $"buffText.buffInfos[{selectedIndex}].name");
        description.SetPath(state == BuffSelector.State.Inactive ? "buffText.blank.description" : $"buffText.buffInfos[{selectedIndex}].description");

    }
}
