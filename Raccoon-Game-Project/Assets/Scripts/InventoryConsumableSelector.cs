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
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        imageEnabled = image.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        RectTransform currentIndexedSlotObject = rectTransform.parent.Find(SLOT_NAME + selectionIndex) as RectTransform;
        if (currentIndexedSlotObject != null)
        {
            rectTransform.position = Vector3.Lerp(rectTransform.position, currentIndexedSlotObject.position, Time.deltaTime * ANIMATION_SCALE);
        }
        if (IsSwitchPressed())
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
        if (IsDownPressed())
        {
            selectionIndex += rowWidth;
            selectionIndex %= numSlots;
        }
        else if (IsUpPressed())
        {
            selectionIndex -= rowWidth;
            if (selectionIndex < 0)
            {
                selectionIndex = numSlots + selectionIndex;
            }
        }
        else if (IsRightPressed())
        {
            selectionIndex++;
            selectionIndex %= numSlots;
        }
        else if (IsLeftPressed())
        {
            selectionIndex--;
            if (selectionIndex < 0)
            {
                selectionIndex = numSlots + selectionIndex;
            }
        }
    }
    bool IsDownPressed() 
    {
        return Input.GetKeyDown(KeyCode.DownArrow) ||
            Input.GetKeyDown(KeyCode.S);
    }

    bool IsUpPressed() 
    {
        return Input.GetKeyDown(KeyCode.UpArrow)   ||
            Input.GetKeyDown(KeyCode.W);
    }

    bool IsRightPressed()
    {
        return Input.GetKeyDown(KeyCode.RightArrow)    ||
            Input.GetKeyDown(KeyCode.D);
    }

    bool IsLeftPressed()
    {
        return Input.GetKeyDown(KeyCode.LeftArrow)     ||
            Input.GetKeyDown(KeyCode.A);
    }
    bool IsSwitchPressed() 
    {
        return Input.GetButtonDown("Fire1") ||
            Input.GetButtonDown("Fire2") ||
            Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.LeftControl);
    }
}
