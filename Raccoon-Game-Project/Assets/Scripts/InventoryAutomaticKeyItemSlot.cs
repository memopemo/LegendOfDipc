using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryAutomaticKeyItemSlot : MonoBehaviour
{
    int index = 0;
    Image sprite;
    private void Start()
    {
        sprite = GetComponent<Image>();
        index = Inventory.GetIndexFromName(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        sprite.enabled = SaveManager.GetSave().ObtainedKeyUnselectableItems[index];
    }
}
