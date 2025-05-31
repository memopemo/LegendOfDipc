using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class DemonBuffSelector : MonoBehaviour
{
    const int rowWidth = 7;
    const int numSlots = 49;
    float ANIMATION_SCALE = 25;
    public int selectedIndex;
    [NonSerialized] RectTransform rectTransform;
    const string SLOT_NAME = "Buff_";
    BuffTipWindow buffTipWindow;
    [SerializeField] AudioClip[] sounds;
    float TPTextColorAnim;
    [SerializeField] TMP_Text TPText;
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
        if (UIInput.IsConfirmPressed)
        {
            if (currentIndexedSlotObject.GetComponent<BuffSelector>().state == BuffSelector.State.Selectable
                && !DemonBuffs.IsFull()
                && SaveManager.GetSave().ToiletPaperRolls > 0)
            {
                SaveManager.GetSave().ToiletPaperRolls--;
                DemonBuffs.AddBuff((DemonBuffs.DemonBuff)(selectedIndex + 1));
                GameObject buffChosenGhost = Instantiate(gameObject, currentIndexedSlotObject);
                buffChosenGhost.transform.position = currentIndexedSlotObject.transform.position;
                Destroy(buffChosenGhost.GetComponent<DemonBuffSelector>());
                PlaySound(2);
            }
            else
            {
                //TODO: play red anim on pendants too to show other reasons why it cant be selected.
                TPTextColorAnim = 1;
                PlaySound(3);
            }
        }

        if (UIInput.IsBackPressed)
        {
            FindFirstObjectByType<DemonBuffMenu>().Toggle();
        }
        if (UIInput.IsDownPressed)
        {
            selectedIndex += rowWidth;
            selectedIndex %= numSlots;
            buffTipWindow.SetSelectedBuff(selectedIndex);
            PlaySound(0);
        }
        else if (UIInput.IsUpPressed)
        {
            selectedIndex -= rowWidth;
            if (selectedIndex < 0)
            {
                selectedIndex = numSlots + selectedIndex;
            }
            buffTipWindow.SetSelectedBuff(selectedIndex);
            PlaySound(0);
        }
        else if (UIInput.IsRightPressed)
        {
            selectedIndex++;
            selectedIndex %= numSlots;
            buffTipWindow.SetSelectedBuff(selectedIndex);
            PlaySound(1);
        }
        else if (UIInput.IsLeftPressed)
        {
            selectedIndex--;
            if (selectedIndex < 0)
            {
                selectedIndex = numSlots + selectedIndex;
            }
            buffTipWindow.SetSelectedBuff(selectedIndex);
            PlaySound(1);
        }
        Buttons.UpdateAxis();
        TPTextColorAnim -= Time.deltaTime;
        TPText.color = Color.Lerp(Color.white, Color.red, TPTextColorAnim);

    }
    void PlaySound(int i)
    {
        AudioSource.PlayClipAtPoint(sounds[i], FindFirstObjectByType<Camera>().transform.position + Vector3.back);
    }
}
