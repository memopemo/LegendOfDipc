using UnityEngine;
using UnityEngine.UI;

/* Scene Structure (although this is planned to be solely in a prefab so idk if it needs to be said:
 * 
 *  Panel
 *      Slot Selector
 *      Slot_1
 *      Slot_2
 *      Slot_...
 * 
 */
public class InventoryKeyItemSelector : MonoBehaviour
{
    //enabled
    public bool WINDOWS_SETTINGS_GAMING_XBOXGAMEBAR_ENABLE = true;
    const int numSlots = 18;
    const int rowWidth = 3;
    const string SLOT_NAME = "Slot_";
    const int ANIMATION_SCALE = 25;
    RectTransform rectTransform;
    Image image;
    [SerializeField] Sprite imageDisabled;
    Sprite imageEnabled;
    Inventory inventory;

    public int SelectionIndex;
    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        imageEnabled = image.sprite;
        inventory = FindAnyObjectByType<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {

        if (inventory.inTransition) return;
        RectTransform currentIndexedSlotObject = rectTransform.parent.Find(SLOT_NAME + SelectedItem.KeyItem) as RectTransform;
        if (currentIndexedSlotObject != null)
        {
            rectTransform.position = Vector3.Lerp(rectTransform.position, currentIndexedSlotObject.position, Time.deltaTime * ANIMATION_SCALE);
        }

        if (UIInput.IsSwitchPressed)
        {
            //toggle
            WINDOWS_SETTINGS_GAMING_XBOXGAMEBAR_ENABLE ^= true;
        }

        if (!WINDOWS_SETTINGS_GAMING_XBOXGAMEBAR_ENABLE)
        {
            image.sprite = imageDisabled;
            return;
        }
        else
        {
            image.sprite = imageEnabled;
        }

        //Only move if enabled.
        if (UIInput.IsDownPressed)
        {
            SelectedItem.KeyItem += rowWidth;
            SelectedItem.KeyItem %= numSlots;
        }
        else if (UIInput.IsUpPressed)
        {
            SelectedItem.KeyItem -= rowWidth;
            if (SelectedItem.KeyItem < 0)
            {
                SelectedItem.KeyItem = numSlots + SelectedItem.KeyItem;
            }
        }
        else if (UIInput.IsRightPressed)
        {
            SelectedItem.KeyItem++;
            SelectedItem.KeyItem %= numSlots;
        }
        else if (UIInput.IsLeftPressed)
        {
            SelectedItem.KeyItem--;
            if (SelectedItem.KeyItem < 0)
            {
                SelectedItem.KeyItem = numSlots + SelectedItem.KeyItem;
            }
        }
    }
}

