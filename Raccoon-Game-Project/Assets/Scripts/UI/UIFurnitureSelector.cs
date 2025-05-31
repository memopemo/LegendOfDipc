using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIFurnitureSelector : MonoBehaviour
{
    public int selected;
    bool isSelectingDestroy;
    bool isActive = false;
    public FurnitureList furnitureList;
    public List<int> obtainedFurniture;
    SaveFile saveFile;
    float x;
    float z;
    [SerializeField] UIFurnitureSelectorSlot CurrentSlot;
    [SerializeField] UIFurnitureSelectorSlot AheadSlot; //slot physically below current
    [SerializeField] UIFurnitureSelectorSlot BehindSlot; //slot physcally above current
    [SerializeField] UIFurnitureSelectorSlot AnimSlot; //either end for transitioning

    SpriteRenderer selector;

    List<float> initialPos;

    //info
    [SerializeField] UILanguagedText titleText;
    [SerializeField] UILanguagedText descText;
    [SerializeField] TMP_Text sizeText;
    [SerializeField] SpriteRenderer iconPlacementType;
    [SerializeField] SpriteRenderer iconThemeType;
    [SerializeField] SpriteRenderer iconSpecial;
    [SerializeField] SpriteRenderer iconInteractable;
    [SerializeField] Sprite[] placementIcons;
    [SerializeField] Sprite[] themeIcons;
    [SerializeField] UILanguagedText iconHelper;
    int helperIndex = 3;

    List<Vector3> initPositions;

    public void Activate()
    {
        StopAllCoroutines();
        StartCoroutine(ActivateCO());

    }
    public void Deactivate()
    {
        StopAllCoroutines();
        StartCoroutine(DeactivateCO());
    }
    public void Awake()
    {
        saveFile = SaveManager.GetSave();
        for (int i = 0; i < saveFile.HouseItems.Length; i++)
        {
            if (!saveFile.HouseItems[i]) continue;
            obtainedFurniture.Add(i + 1); //furniture 0 is blank.
        }

    }
    public void Start()
    {
        x = CurrentSlot.transform.localPosition.x; //x position does not change.
        z = CurrentSlot.transform.localPosition.z;
        initialPos = new()
        {
            CurrentSlot.transform.localPosition.y,
            AheadSlot.transform.localPosition.y,
            BehindSlot.transform.localPosition.y,
            AnimSlot.transform.localPosition.y,
            iconHelper.transform.localPosition.x,
        };
        selector = transform.GetChild(0).GetComponent<SpriteRenderer>();
        initPositions = new();
        for (int i = 0; i < transform.childCount; i++)
        {
            initPositions.Add(transform.GetChild(i).localPosition);
            transform.GetChild(i).localPosition = Vector3.zero;
            transform.GetChild(i).gameObject.SetActive(false);
        }
        UpdateSlotIcons();
    }
    IEnumerator ActivateCO()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
        float transitionTime = 0;
        while (0.3 - transitionTime > 0)
        {
            transitionTime += Time.deltaTime;
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform item = transform.GetChild(i);
                item.localPosition = Vector3.Lerp(item.localPosition, initPositions[i], transitionTime);
            }
            yield return null;
        }
        isActive = true;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform item = transform.GetChild(i);
            item.localPosition = initPositions[i];
        }
        
    }
    IEnumerator DeactivateCO()
    {
        isActive = false;
        float transitionTime = 0;
        while (0.3 - transitionTime > 0)
        {
            transitionTime += Time.deltaTime;
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform item = transform.GetChild(i);
                item.localPosition = Vector3.Lerp(item.localPosition, Vector2.zero, transitionTime);
            }
            yield return null;
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform item = transform.GetChild(i);
            item.gameObject.SetActive(false);

        }
    }
    public void Update()
    {
        if (!isActive) return;
        Furniture furniture = furnitureList.allFurniture[obtainedFurniture[selected]];

        //inputs
        if (UIInput.IsDownPressed && !isSelectingDestroy)
        {
            StopAllCoroutines();
            selected++;
            UpdateSlotIcons();
            StartCoroutine(MoveDown());
            ClearIconHelper();
        }
        if (UIInput.IsUpPressed && !isSelectingDestroy)
        {
            StopAllCoroutines();
            selected--;
            UpdateSlotIcons();
            StartCoroutine(MoveUp());
            ClearIconHelper();
        }
        if(UIInput.IsRightPressed && !isSelectingDestroy)
        {
            helperIndex++;
            if(helperIndex == 0 && furniture.theme == Furniture.Theme.None) helperIndex++;
            if(helperIndex == 1 && furniture.type == Furniture.PlacementType.None) helperIndex++;
            if(helperIndex == 2 && !furniture.isInteractable) helperIndex++;
            helperIndex %= 4;
            switch (helperIndex)
            {
                case 0: //theme
                    // i have to enable and disable the text and sprite by themselves or else theres a noticable frame where they show the wrong sprite.
                    iconHelper.GetComponent<TMP_Text>().enabled = true;
                    iconHelper.GetComponentInChildren<SpriteRenderer>().enabled = true;
                    iconHelper.SetPath($"furniture.selectingFurniture.iconHelper.theme[{(int)furniture.theme}].thing");
                    break;
                case 1: //type
                    iconHelper.SetPath($"furniture.selectingFurniture.iconHelper.type[{(int)furniture.type}].thing");
                    break;
                case 2: //interactable
                    iconHelper.SetPath($"furniture.selectingFurniture.iconHelper.interactable");
                    break;
                case 3: //off
                    ClearIconHelper();
                    break;
            }
        }
        if (UIInput.IsRightPressed && isSelectingDestroy)
        {
            isSelectingDestroy = false;
        }
        if (UIInput.IsLeftPressed && !isSelectingDestroy)
        {
            isSelectingDestroy = true;
            ClearIconHelper();
        }
        if (UIInput.IsBackPressed)
        {
            Deactivate();
            GetComponent<FurnitureChest>().Cancel();
        }
        if (UIInput.IsConfirmPressed)
        {
            Deactivate();
            GetComponent<FurnitureChest>().Yay(isSelectingDestroy ? 0 : selected + 1);
        }

        //update selector position and size
        selector.transform.localPosition = new Vector3(Mathf.Lerp(selector.transform.localPosition.x, isSelectingDestroy ? -6.5f : -3.5f, Time.deltaTime * 10),
                                                    selector.transform.localPosition.y,
                                                    selector.transform.localPosition.z);
        selector.size = Vector2.Lerp(selector.size, Vector2.one * (isSelectingDestroy ? 1.5f : 2.5f), Time.deltaTime * 10);

        //set info text
        titleText.SetPath(isSelectingDestroy ? "furniture.selectingFurniture.remove.title" : $"furniture.info[{obtainedFurniture[selected]}].title");
        descText.SetPath(isSelectingDestroy ? "furniture.selectingFurniture.remove.description" : $"furniture.info[{obtainedFurniture[selected]}].description");
        sizeText.text = isSelectingDestroy ? "---" : $"{furniture.size.x}x{furniture.size.y}";

        //update icons
        iconInteractable.enabled = !isSelectingDestroy && furniture.isInteractable;
        iconSpecial.enabled = !isSelectingDestroy && furniture.isSpeccial;
        iconPlacementType.sprite = isSelectingDestroy ? placementIcons[0] : placementIcons[(int)furniture.type];
        iconThemeType.sprite = isSelectingDestroy ? placementIcons[0] : themeIcons[(int)furniture.theme];

        //position icon helper
        iconHelper.transform.localPosition = new Vector3(initialPos[4]+(helperIndex*0.5f), iconHelper.transform.localPosition.y, iconHelper.transform.localPosition.z);
    }

    
    public void ClearIconHelper()
    {
        helperIndex = 3;
        iconHelper.SetPath("blank");
        iconHelper.GetComponent<TMP_Text>().enabled = false;
        iconHelper.GetComponentInChildren<SpriteRenderer>().enabled = false;
    }

    public void UpdateSlotIcons()
    {
        selected = (obtainedFurniture.Count + selected) % obtainedFurniture.Count;
        CurrentSlot.SetItem(furnitureList.allFurniture[obtainedFurniture[(obtainedFurniture.Count + selected) % obtainedFurniture.Count]].icon);
        AheadSlot.SetItem(furnitureList.allFurniture[obtainedFurniture[(obtainedFurniture.Count + selected + 1) % obtainedFurniture.Count]].icon);
        BehindSlot.SetItem(furnitureList.allFurniture[obtainedFurniture[(obtainedFurniture.Count + (selected - 1)) % obtainedFurniture.Count]].icon);
    }
    IEnumerator MoveDown()
    {
        //goal: start down 1 slot and move up
        float targetY = 0;
        float lerpedY = 1;
        AnimSlot.SetItem(furnitureList.allFurniture[obtainedFurniture[(obtainedFurniture.Count + selected - 2) % obtainedFurniture.Count]].icon);

        while (lerpedY - targetY > 0.01f)
        {
            lerpedY = Mathf.Lerp(lerpedY, targetY, Time.deltaTime * 10f);

            //set position
            CurrentSlot.transform.localPosition = new Vector3(x, initialPos[0] - lerpedY * 3, z);
            AheadSlot.transform.localPosition = new Vector3(x, initialPos[1] - lerpedY * 3, z);
            BehindSlot.transform.localPosition = new Vector3(x, initialPos[2] - lerpedY * 3, z);
            AnimSlot.transform.localPosition = new Vector3(x, initialPos[3] - lerpedY * 3, z);

            //set alpha
            CurrentSlot.SetAlpha(1 + (lerpedY * 0.5f)); //1 to 0.5
            BehindSlot.SetAlpha(0.5f + (lerpedY * 0.5f)); //0 to 0.5
            AheadSlot.SetAlpha(0.5f - (lerpedY * 0.5f)); //0.5 to 1
            AnimSlot.SetAlpha(lerpedY * 0.5F); // 0.5 to 0
            yield return null;
        }
    }
    IEnumerator MoveUp()
    {
        //goal: start down 1 slot and move up
        float targetY = 0;
        float lerpedY = 1;
        AnimSlot.SetItem(furnitureList.allFurniture[obtainedFurniture[(obtainedFurniture.Count + selected + 2) % obtainedFurniture.Count]].icon);
        while (lerpedY - targetY > 0.01f)
        {
            lerpedY = Mathf.Lerp(lerpedY, targetY, Time.deltaTime * 10f);

            //set position
            CurrentSlot.transform.localPosition = new Vector3(x, initialPos[0] + lerpedY * 3, z);
            AheadSlot.transform.localPosition = new Vector3(x, initialPos[1] + lerpedY * 3, z);
            BehindSlot.transform.localPosition = new Vector3(x, initialPos[2] + lerpedY * 3, z);
            AnimSlot.transform.localPosition = new Vector3(x, initialPos[3] + (3 * -4) + lerpedY * 3, z);

            //set alpha
            CurrentSlot.SetAlpha(1 + (lerpedY * 0.5f)); //
            AheadSlot.SetAlpha(0.5f + (lerpedY * 0.5f));
            BehindSlot.SetAlpha(0.5f - (lerpedY * 0.5f));
            AnimSlot.SetAlpha(lerpedY * 0.5F);
            yield return null;
        }
    }
}
