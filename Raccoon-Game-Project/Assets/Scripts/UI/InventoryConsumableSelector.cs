using System;
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
        if (inventory.inTransition) return;
        RectTransform currentIndexedSlotObject = rectTransform.parent.Find(SLOT_NAME + SelectedItem.ConsumableItem) as RectTransform;
        if (currentIndexedSlotObject != null)
        {
            rectTransform.position = Vector3.Lerp(rectTransform.position, currentIndexedSlotObject.position, Time.deltaTime * ANIMATION_SCALE);
        }
        if (UIInput.IsSwitchPressed)
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
        if (UIInput.IsDownPressed)
        {
            SelectedItem.ConsumableItem += rowWidth;
            SelectedItem.ConsumableItem %= numSlots;
        }
        else if (UIInput.IsUpPressed)
        {
            SelectedItem.ConsumableItem -= rowWidth;
            if (SelectedItem.ConsumableItem < 0)
            {
                SelectedItem.ConsumableItem = numSlots + SelectedItem.ConsumableItem;
            }
        }
        else if (UIInput.IsRightPressed)
        {
            SelectedItem.ConsumableItem++;
            SelectedItem.ConsumableItem %= numSlots;
        }
        else if (UIInput.IsLeftPressed)
        {
            SelectedItem.ConsumableItem--;
            if (SelectedItem.ConsumableItem < 0)
            {
                SelectedItem.ConsumableItem = numSlots + SelectedItem.ConsumableItem;
            }
        }
    }
}
