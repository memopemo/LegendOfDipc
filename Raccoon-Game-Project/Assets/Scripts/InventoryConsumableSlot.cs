using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.Windows;

public class InventoryConsumableSlot : MonoBehaviour
{
    int index = 0;
    private void Start()
    {
        //get index via gameobject name :)
        index = Inventory.GetIndexFromName(gameObject); //dont ask, copied from internet
    }
    private void Update()
    {

        foreach(var i in transform.GetComponentsInChildren<TMP_Text>()) 
        {
            i.text = SaveManager.GetSave().InventoryConsumableCount[index].ToString();
            i.enabled  = i.text != "0";
        }
    }
}
