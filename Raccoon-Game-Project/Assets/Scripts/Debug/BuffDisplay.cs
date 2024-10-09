using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BuffDisplay : MonoBehaviour
{
    [SerializeField] int buffToDisplay; //buff slot 1, 2, or 3?
    [SerializeField] Sprite[] graphics;
    Image image;
    TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        text = transform.GetChild(0).GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        image.sprite = graphics[(int)DemonBuffs.demonBuffs[buffToDisplay]];
        text.text = TimeConverter.SecsToDisplayMinSecs(DemonBuffs.buffTimers[buffToDisplay]);
        text.enabled = DemonBuffs.buffTimers[buffToDisplay] != 0;
    }
}
