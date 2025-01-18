using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UISelectionPage : MonoBehaviour
{
    public UISelectable currentlySelected;

    // Update is called once per frame
    void Update()
    {
        if (UIInput.IsConfirmReleased)
        {
            currentlySelected.OnConfirm.Invoke();
        }
        if (UIInput.IsBackReleased || Buttons.IsButtonUp(Buttons.Pause))
        {
            currentlySelected.OnCancel.Invoke();
        }
        if (UIInput.IsUpPressed)
        {
            currentlySelected.OnUp.Invoke();
        }
        if (UIInput.IsDownPressed)
        {
            currentlySelected.OnDown.Invoke();
        }
        if (UIInput.IsLeftPressed)
        {
            currentlySelected.OnLeft.Invoke();
        }
        if (UIInput.IsRightPressed)
        {
            currentlySelected.OnRight.Invoke();
        }
        Buttons.UpdateAxis();
    }
    public void Select(UISelectable selectable)
    {
        currentlySelected = selectable;
    }
}
