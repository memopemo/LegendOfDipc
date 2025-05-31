using UnityEngine;
using UnityEngine.UI;

public class HUDKeyItem : MonoBehaviour
{
    [SerializeField] ItemSpriteList itemPreviews;
    Image image;
    Image flash;
    float timeActive = 1;
    int currentItem;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        flash = transform.parent.GetChild(0).GetComponent<Image>();
        currentItem = SelectedItem.KeyItem;
    }

    // Update is called once per frame
    void Update()
    {
        image.sprite = itemPreviews.keyItems[SelectedItem.KeyItem];
        image.enabled = SaveManager.GetSave().ObtainedKeyItems[SelectedItem.KeyItem];

        int item = SelectedItem.KeyItem;
        if (currentItem != item)
        {
            timeActive = 0;
        }
        if (timeActive < 1)
        {
            timeActive += Time.deltaTime;
        }
        currentItem = item;
        flash.color = new Color(1, 1, 1, 1 - timeActive);
    }
}
