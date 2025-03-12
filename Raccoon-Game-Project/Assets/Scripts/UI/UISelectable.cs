using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UISelectable : MonoBehaviour
{
    Sprite initial;
    [SerializeField] Sprite selected;
    [SerializeField] Sprite pressed;
    Image image;
    public UnityEvent OnConfirm;
    public UnityEvent OnCancel;
    public UnityEvent OnUp;
    public UnityEvent OnDown;
    public UnityEvent OnLeft;
    public UnityEvent OnRight;
    UISelectionPage uISelectionPage;

    void Start()
    {
        image = GetComponent<Image>();
        uISelectionPage = GetComponentInParent<UISelectionPage>();
        initial = image.sprite;

    }
    void Update()
    {
        image.sprite = (uISelectionPage.currentlySelected == this) switch
        {
            true when IsConfirmHeldDown() => pressed,
            true => selected,
            _ => initial,
        };
    }

    bool IsConfirmHeldDown() => Buttons.IsButtonHeld(Buttons.Sword) || Input.GetKey(KeyCode.Return) ||
        Input.GetKey(KeyCode.Space);

}
