using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.Windows;

public class InventoryConsumableSlot : MonoBehaviour
{
    int index = 0;
    Image image;
    Inventory inventory;
    private void Start()
    {
        //get index via gameobject name :)
        index = GameObjectParser.GetIndexFromName(gameObject); //dont ask, copied from internet
        image = GetComponent<Image>();
        inventory = FindAnyObjectByType<Inventory>();
    }
    private void Update()
    {
        image.sprite = inventory.itemPreviews.consumableItems[SaveManager.GetSave().InventoryConsumableType[index]];
        foreach(var i in transform.GetComponentsInChildren<TMP_Text>()) 
        {
            i.text = SaveManager.GetSave().InventoryConsumableCount[index].ToString();
            i.enabled  = i.text != "0";
        }
    }
}
