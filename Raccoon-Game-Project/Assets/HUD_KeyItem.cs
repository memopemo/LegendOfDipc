using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDKeyItem : MonoBehaviour
{
   [SerializeField] ItemSpriteList itemPreviews;
    Image image;
    InventoryKeyItemSelector inventoryKeyItemSelector;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        inventoryKeyItemSelector = FindAnyObjectByType<InventoryKeyItemSelector>(FindObjectsInactive.Include);
    }

    // Update is called once per frame
    void Update()
    {
        image.sprite = itemPreviews.keyItems[inventoryKeyItemSelector.SelectionIndex];
        image.enabled = SaveManager.GetSave().ObtainedKeyItems[inventoryKeyItemSelector.SelectionIndex];
    }
}
