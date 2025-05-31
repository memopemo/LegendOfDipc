using UnityEngine;

public class UIFurnitureSelectorSlot : MonoBehaviour
{
    SpriteRenderer icon;
    SpriteRenderer border;
    public void Start()
    {
        icon = transform.GetChild(1).GetComponent<SpriteRenderer>();
        border = GetComponent<SpriteRenderer>();
    }
    public void SetItem(Sprite sprite)
    {
        icon.sprite = sprite;
        icon.transform.localPosition = -icon.localBounds.center;
    }
    public void SetAlpha(float alpha)
    {
        icon.color = new Color(icon.color.r, icon.color.g, icon.color.b, alpha);
        border.color = new Color(border.color.r, border.color.g, border.color.b, alpha);
    }
}
