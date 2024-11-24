using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] GameObject thrownShopItem;
    List<Vector2> outstrectchedPositions = new();
    bool isOpen = false;
    int selectedSlotIndex;
    int[] shopItemIDs = new int[4];
    int[] shopItemPrices = new int[4];
    [SerializeField] ItemSpriteList itemSpriteList;
    float timeShaking;
    const int ITEM_IMAGE_SUBCHILD_INDEX = 0;
    const int PRICE_SUBCHILD_INDEX = 1;
    public void Start()
    {
        string[][] shopInfoFile = CSVReader.GetCSV("Shops");

        //initialize shop info
        for (int shopSlotChildIndex = 0; shopSlotChildIndex < 4; shopSlotChildIndex++)
        {
            int row = (GameObjectParser.GetIndexFromName(gameObject) * 4) + shopSlotChildIndex + 1;
            shopItemIDs[shopSlotChildIndex] = int.Parse(shopInfoFile[row][0]);
            shopItemPrices[shopSlotChildIndex] = int.Parse(shopInfoFile[row][1]);
        }

        //initialize all children
        for (int shopAllChildIndex = 0; shopAllChildIndex < 5; shopAllChildIndex++)
        {
            Transform child = transform.GetChild(shopAllChildIndex);
            outstrectchedPositions.Insert(shopAllChildIndex, transform.GetChild(shopAllChildIndex).localPosition);
            child.localPosition = Vector2.zero;
            child.gameObject.SetActive(false);
            if (shopAllChildIndex == 4) continue;
            child.GetChild(ITEM_IMAGE_SUBCHILD_INDEX).GetComponent<SpriteRenderer>().sprite = itemSpriteList.consumableItems[shopItemIDs[shopAllChildIndex]];
            child.GetChild(PRICE_SUBCHILD_INDEX).GetComponent<TMP_Text>().text = shopItemPrices[shopAllChildIndex].ToString();

        }
    }
    public void Update()
    {
        for (int childIndex = 0; childIndex < 5; childIndex++)
        {
            Transform child = transform.GetChild(childIndex);
            if (childIndex == 4)
            {
                child.localPosition = Vector2.Lerp(child.localPosition, transform.GetChild(selectedSlotIndex).localPosition, Time.deltaTime * 10);
                child.localPosition += Vector3.back * 2;
                continue;
            }
            child.localPosition = Vector2.Lerp(child.localPosition, isOpen ? outstrectchedPositions[childIndex] : Vector2.zero, Time.deltaTime * 10);
            child.localPosition += Vector3.back * 3;
        }

        if (isOpen)
        {
            if (Buttons.AxisDown == Vector2Int.right)
            {
                selectedSlotIndex += 1;
                selectedSlotIndex %= 4;
            }
            if (Buttons.AxisDown == Vector2Int.left)
            {
                selectedSlotIndex -= 1;
                if (selectedSlotIndex == -1) selectedSlotIndex = 3;
            }
            if (Buttons.IsButtonDown(Buttons.Sword))
            {
                Decide();
            }
            if (Buttons.IsButtonDown(Buttons.KeyItem))
            {
                OnClose();
            }
        }

    }

    public void OnInteract()
    {
        isOpen = true;
        FreezeManager.FreezeAll<CutSceneFreezer>();
        Show();

    }
    public void OnClose()
    {
        isOpen = false;
        FreezeManager.UnfreezeAll<CutSceneFreezer>();
        Invoke(nameof(DelayedHide), 0.2f);
    }

    void DelayedHide()
    {
        for (int childIndex = 0; childIndex < 5; childIndex++)
        {
            Transform child = transform.GetChild(childIndex);
            child.gameObject.SetActive(false);
        }
    }

    void Show()
    {
        for (int slotChildIndex = 0; slotChildIndex < 5; slotChildIndex++)
        {
            Transform child = transform.GetChild(slotChildIndex);
            child.gameObject.SetActive(true);
        }
    }


    public void Decide()
    {
        if (shopItemIDs[selectedSlotIndex] != 0 && SaveManager.GetSave().Money >= shopItemPrices[selectedSlotIndex])
        {
            SaveManager.GetSave().Money -= shopItemPrices[selectedSlotIndex];
            GameObject instance = Instantiate(thrownShopItem, transform.position, transform.rotation);
            instance.GetComponent<ThrownShopItem>().id = shopItemIDs[selectedSlotIndex];
            instance.GetComponent<SpriteRenderer>().sprite = itemSpriteList.consumableItems[shopItemIDs[selectedSlotIndex]];
        }
        else
        {
            StartCoroutine(nameof(DenyOption));
        }
    }
    IEnumerator DenyOption()
    {
        Vector3 initialXPos = transform.GetChild(selectedSlotIndex).localPosition;
        float timeShake = 0.25f;
        while (timeShake > 0)
        {
            timeShake -= Time.deltaTime;
            transform.GetChild(selectedSlotIndex).localPosition = new Vector3(initialXPos.x + (Mathf.Sin(timeShake * 30) / 3), initialXPos.y, initialXPos.z);
            yield return null;
        }
    }
}
