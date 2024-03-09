using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryBombCount : MonoBehaviour
{
    TMP_Text text;
    private void Start()
    {
        text = GetComponent<TMP_Text>();
    }
    private void Update()
    {
        //count number of trues in it.
        text.text = SaveManager.GetSave().ComputerBombs.ToString();
    }
}
