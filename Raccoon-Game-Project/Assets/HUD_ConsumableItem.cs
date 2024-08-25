using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDConsumableItem : MonoBehaviour
{
    [SerializeField] ItemSpriteList itemPreviews;
    Image image;
    InventoryConsumableSelector inventoryConsumableSelector;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        inventoryConsumableSelector = FindAnyObjectByType<InventoryConsumableSelector>(FindObjectsInactive.Include);
    }

    // Update is called once per frame
    void Update()
    {
        image.sprite = itemPreviews.consumableItems[SaveManager.GetSave().InventoryConsumableType[inventoryConsumableSelector.selectionIndex]];
    }
}
