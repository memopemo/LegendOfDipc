using UnityEngine;
using UnityEngine.UI;

public class InventoryShieldSlot : MonoBehaviour
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
        int index = SaveManager.GetSave().CurrentShield;
        if (index < 0)
        {
            spriteRenderer.sprite = empty;
            return;
        }
        spriteRenderer.sprite = itemSpriteList.shields[index];
    }
}
