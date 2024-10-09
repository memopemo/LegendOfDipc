using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGItemSelectorWindow : MonoBehaviour
{
    public bool isConsumableMode;
    public bool isActive;
    public bool canLoadAmountText;
    public ItemSpriteList itemSpriteList;
    SpriteRenderer spriteRenderer;
    public const int CONSUMABLE_WIDTH = 9;
    public const int KEY_WIDTH = 7;
    private const int SPEED = 10;

    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Update()
    {
        float width = (isActive ? 1 : 0) * (isConsumableMode ? CONSUMABLE_WIDTH : KEY_WIDTH); //can be 0, key_width, or consumable_width.
        spriteRenderer.size = Vector2.Lerp(spriteRenderer.size, new Vector2(width, spriteRenderer.size.y), Time.deltaTime * SPEED);
        spriteRenderer.enabled = spriteRenderer.size.x >= 1; //for making the border shrink not look weird
        canLoadAmountText = spriteRenderer.size.x + 1 >= CONSUMABLE_WIDTH;
    }

}
