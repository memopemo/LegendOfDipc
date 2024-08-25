using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RPGItemSlot : MonoBehaviour
{
    int index; //new index will need to be calculated relative to the column width.
    const int CONSUMABLE_WIDTH = 4;
    const int KEY_WIDTH = 3;
    int keyItemIndex;
    SpriteRenderer itemDisplay;
    RPGItemSelectorWindow rPGItemSelectorWindow;
    TMP_Text amountDisplay;

    // Start is called before the first frame update
    void Start()
    {
        index = GameObjectParser.GetIndexFromName(gameObject);
        keyItemIndex = index - (index / CONSUMABLE_WIDTH); //math to shorten the selectable area.
        itemDisplay = transform.GetChild(0).GetComponent<SpriteRenderer>();
        rPGItemSelectorWindow = FindAnyObjectByType<RPGItemSelectorWindow>();
        amountDisplay = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    public void Update()
    {
        
        if(rPGItemSelectorWindow.isConsumableMode)
        {
            itemDisplay.sprite = rPGItemSelectorWindow.itemSpriteList.consumableItems[SaveManager.GetSave().InventoryConsumableType[index]];
        }
        else if(index % 4 != 3) //disable updates on anything out of bounds when updating key items.
        {
            itemDisplay.sprite = rPGItemSelectorWindow.itemSpriteList.keyItems[keyItemIndex];
            itemDisplay.enabled = SaveManager.GetSave().ObtainedKeyItems[keyItemIndex];
            
        }
        amountDisplay.text =  SaveManager.GetSave().InventoryConsumableCount[index].ToString();
        amountDisplay.enabled = rPGItemSelectorWindow.isActive && rPGItemSelectorWindow.canLoadAmountText && amountDisplay.text != "0";
        
    }
}
