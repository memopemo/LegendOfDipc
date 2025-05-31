using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDConsumableItem : MonoBehaviour
{
    [SerializeField] bool isKeyItem;
    [SerializeField] ItemSpriteList itemPreviews;
    Image image;
    Image flash;
    float timeActive = 1;
    int currentItem;
    [SerializeField] Sprite gamepad;
    [SerializeField] Sprite keyboard;
    [SerializeField] TMP_Text buttonText;
    [SerializeField] Image buttonBorder;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        flash = transform.parent.GetChild(0).GetComponent<Image>();
        currentItem = SelectedItem.ConsumableItem;
    }

    // Update is called once per frame
    void Update()
    {
        int item = isKeyItem ? SelectedItem.KeyItem : SelectedItem.ConsumableItem;
        if (currentItem != item)
        {
            timeActive = 0;
        }
        if (timeActive < 1)
        {
            timeActive += Time.deltaTime;
        }
        currentItem = item;
        if (isKeyItem)
        {
            image.sprite = itemPreviews.keyItems[SelectedItem.KeyItem];
            image.enabled = SaveManager.GetSave().ObtainedKeyItems[SelectedItem.KeyItem];
        }
        else
        {
            image.sprite = itemPreviews.consumableItems[SaveManager.GetSave().InventoryConsumableType[currentItem]];
        }

        flash.color = new Color(1, 1, 1, 1 - timeActive);

        buttonBorder.sprite = Buttons.isUsingButtons ? gamepad : keyboard;

        Button button = isKeyItem ? Buttons.KeyItem : Buttons.ConsumableItem;

        string buttonName = Buttons.isUsingButtons ? button.ControllerName : button.KeyName.ToString();
        buttonText.text = buttonName;
        foreach (var text in buttonText.GetComponentsInChildren<TMP_Text>())
        {
            text.text = buttonName;
        }
    }
}
