using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InventoryHeartContainerCount : MonoBehaviour
{
    TMP_Text text;
    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }
    private void Update()
    {
        //count number of trues in it.
        text.text = SaveManager.GetSave().HeartContainersCollected.Count(c => c).ToString();
    }
}
