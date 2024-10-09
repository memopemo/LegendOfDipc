using UnityEngine;
using UnityEngine.UI;

public class HUDKeyItem : MonoBehaviour
{
    [SerializeField] ItemSpriteList itemPreviews;
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.sprite = itemPreviews.keyItems[SelectedItem.KeyItem];
        image.enabled = SaveManager.GetSave().ObtainedKeyItems[SelectedItem.KeyItem];
    }
}
