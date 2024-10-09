using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryArmorSlot : MonoBehaviour
{
    Image spriteRenderer;
    Sprite empty;
    [SerializeField] ItemSpriteList itemSpriteList;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<Image>();
        empty = spriteRenderer.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        int index = SaveManager.GetSave().CurrentArmor;
        if(index < 0)
        {
            spriteRenderer.sprite = empty;
            return;
        }
        spriteRenderer.sprite = itemSpriteList.armors[index];
    }
}
