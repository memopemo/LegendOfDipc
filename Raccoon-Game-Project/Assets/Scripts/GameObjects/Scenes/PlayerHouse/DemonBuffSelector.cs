using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;

public class DemonBuffSelector : MonoBehaviour
{
    const int rowWidth = 7;
    const int numSlots = 49;
    float ANIMATION_SCALE = 25;
    public int selectedIndex;
    [NonSerialized] RectTransform rectTransform;
    const string SLOT_NAME = "Button_";
    BuffTipWindow buffTipWindow;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        selectedIndex = 0;
        buffTipWindow = FindFirstObjectByType<BuffTipWindow>();
        buffTipWindow.SetSelectedBuff(0);
    }

    // Update is called once per frame
    void Update()
    {
        RectTransform currentIndexedSlotObject = rectTransform.parent.Find(SLOT_NAME + selectedIndex) as RectTransform;
        rectTransform.position = Vector3.Lerp(rectTransform.position, currentIndexedSlotObject.position, Time.deltaTime * ANIMATION_SCALE);
        if (Buttons.IsButtonDown(Buttons.Sword) //A
            && currentIndexedSlotObject.GetComponent<BuffSelector>().state == BuffSelector.State.Selectable
            && !DemonBuffs.IsFull()
            && SaveManager.GetSave().ToiletPaperRolls > 0)
        {
            SaveManager.GetSave().ToiletPaperRolls--;
            DemonBuffs.AddBuff((DemonBuffs.DemonBuff)(selectedIndex + 1));
            GameObject buffChosenGhost = Instantiate(gameObject, currentIndexedSlotObject);
            buffChosenGhost.transform.position = currentIndexedSlotObject.transform.position;
            Destroy(buffChosenGhost.GetComponent<DemonBuffSelector>());
        }
        if (Buttons.IsButtonDown(Buttons.KeyItem))
        {
            FindFirstObjectByType<DemonBuffMenu>().Toggle();
        }
        if (UIInput.IsDownPressed)
        {
            selectedIndex += rowWidth;
            selectedIndex %= numSlots;
            buffTipWindow.SetSelectedBuff(selectedIndex);
        }
        else if (UIInput.IsUpPressed)
        {
            selectedIndex -= rowWidth;
            if (selectedIndex < 0)
            {
                selectedIndex = numSlots + selectedIndex;
            }
            buffTipWindow.SetSelectedBuff(selectedIndex);
        }
        else if (UIInput.IsRightPressed)
        {
            selectedIndex++;
            selectedIndex %= numSlots;
            buffTipWindow.SetSelectedBuff(selectedIndex);
        }
        else if (UIInput.IsLeftPressed)
        {
            selectedIndex--;
            if (selectedIndex < 0)
            {
                selectedIndex = numSlots + selectedIndex;
            }
            buffTipWindow.SetSelectedBuff(selectedIndex);
        }
        Buttons.UpdateAxis();
    }
}
