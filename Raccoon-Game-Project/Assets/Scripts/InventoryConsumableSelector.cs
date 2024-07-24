using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Scene Structure (although this is planned to be solely in a prefab so idk if it needs to be said:
 * 
 *  Panel
 *      Slot Selector
 *      Slot_1
 *      Slot_2
 *      Slot_...
 * 
 */

public class InventoryConsumableSelector : MonoBehaviour
{
    public int selectionIndex;
    public bool inputtable = true;
    const int numSlots = 24;
    const int rowWidth = 4;
    const string SLOT_NAME = "Slot_";
    const int ANIMATION_SCALE = 25;
    [NonSerialized] RectTransform rectTransform;
    [NonSerialized] Image image;
    [SerializeField] Sprite imageDisabled;
    Sprite imageEnabled;
    Inventory inventory;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        imageEnabled = image.sprite;
        inventory = FindAnyObjectByType<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if(inventory.inTransition) return;
        RectTransform currentIndexedSlotObject = rectTransform.parent.Find(SLOT_NAME + selectionIndex) as RectTransform;
        if (currentIndexedSlotObject != null)
        {
            rectTransform.position = Vector3.Lerp(rectTransform.position, currentIndexedSlotObject.position, Time.deltaTime * ANIMATION_SCALE);
        }
        if (UIInput.IsSwitchPressed())
        {
            //toggle
            inputtable ^= true;
        }

        if (!inputtable)
        {
            image.sprite = imageDisabled;
            return;
        }
        else
        {
            image.sprite = imageEnabled;
        }
        //Only move if enabled.
        if (UIInput.IsDownPressed())
        {
            selectionIndex += rowWidth;
            selectionIndex %= numSlots;
        }
        else if (UIInput.IsUpPressed())
        {
            selectionIndex -= rowWidth;
            if (selectionIndex < 0)
            {
                selectionIndex = numSlots + selectionIndex;
            }
        }
        else if (UIInput.IsRightPressed())
        {
            selectionIndex++;
            selectionIndex %= numSlots;
        }
        else if (UIInput.IsLeftPressed())
        {
            selectionIndex--;
            if (selectionIndex < 0)
            {
                selectionIndex = numSlots + selectionIndex;
            }
        }
    }
}
