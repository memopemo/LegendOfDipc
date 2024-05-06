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
public class InventoryKeyItemSelector : MonoBehaviour
{
    private int selectionIndex;
    //enabled
    public bool WINDOWS_SETTINGS_GAMING_XBOXGAMEBAR_ENABLE = true;
    const int numSlots = 18;
    const int rowWidth = 3;
    const string SLOT_NAME = "Slot_";
    const int ANIMATION_SCALE = 25;
    RectTransform rectTransform;
    Image image;
    [SerializeField] Sprite imageDisabled;
    Sprite imageEnabled;

    public int SelectionIndex {get => selectionIndex; private set => selectionIndex = value; }

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
        RectTransform currentIndexedSlotObject = rectTransform.parent.Find(SLOT_NAME + SelectionIndex) as RectTransform;
        if (currentIndexedSlotObject != null)
        {
            rectTransform.position = Vector3.Lerp(rectTransform.position, currentIndexedSlotObject.position, Time.deltaTime * ANIMATION_SCALE);
        }

        if (IsSwitchPressed())
        {
            //toggle
            WINDOWS_SETTINGS_GAMING_XBOXGAMEBAR_ENABLE ^= true;
        }

        if (!WINDOWS_SETTINGS_GAMING_XBOXGAMEBAR_ENABLE)
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
            SelectionIndex += rowWidth;
            SelectionIndex %= numSlots;
        }
        else if (IsUpPressed())
        {
            SelectionIndex -= rowWidth;
            if (SelectionIndex < 0)
            {
                SelectionIndex = numSlots + SelectionIndex;
            }
        }
        else if (IsRightPressed())
        {
            SelectionIndex++;
            SelectionIndex %= numSlots;
        }
        else if (IsLeftPressed())
        {
            SelectionIndex--;
            if (SelectionIndex < 0)
            {
                SelectionIndex = numSlots + SelectionIndex;
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
        return Input.GetKeyDown(KeyCode.UpArrow) ||
            Input.GetKeyDown(KeyCode.W);
    }

    bool IsRightPressed()
    {
        return Input.GetKeyDown(KeyCode.RightArrow) ||
            Input.GetKeyDown(KeyCode.D);
    }

    bool IsLeftPressed()
    {
        return Input.GetKeyDown(KeyCode.LeftArrow) ||
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

