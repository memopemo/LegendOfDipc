using System;
using System.Collections;
using System.Collections.Generic;
using Animator2D;
using UnityEngine;

public class Chest : MonoBehaviour
{    
    SimpleAnimator2D simpleAnimator2D;
    [SerializeField] ChestTreasureList chestTreasureList;
    [SerializeField] ItemList itemList; //for only getting the amount of item types.
    [SerializeField] int indexIntoChestTreasureList;
    ChestTreasureList.ChestTreasure chestTreasure;
    void Start()
    {
        simpleAnimator2D = GetComponent<SimpleAnimator2D>();
        GetComponent<Interactable>().OnInteract.AddListener(OpenChest);
        SaveFile sf = SaveManager.GetSave();

        // //bounds checkers
        // if(indexIntoChestTreasureList < 0 || indexIntoChestTreasureList >= sf.OverworldTreasure.Length)
        // {
        //     Debug.LogError($"index {indexIntoChestTreasureList} into Save File Treasure is not in range! (0, {sf.OverworldTreasure.Length})");
        //     SetAsUsed(); //unable to open/looks empty.
        //     return;
        // }
        if(indexIntoChestTreasureList < 0 || indexIntoChestTreasureList >= chestTreasureList.chestTreasure.Length)
        {
            Debug.LogError($"index {indexIntoChestTreasureList} into Chest is not in range! (0, {chestTreasureList.chestTreasure.Length})");
            SetAsUsed(); //unable to open/looks empty.
            return;
        }
        
        //gameplay check if the save already has it as set.
        // if(sf.OverworldTreasure[indexIntoChestTreasureList])
        // {
        //     Debug.Log("Empty because it's already obtained according to save file.");
        //     SetAsUsed(); //unable to open/looks empty.
        //     return;
        // }

        //check if already opened on this save
        chestTreasure = chestTreasureList.chestTreasure[indexIntoChestTreasureList];
    }
    void SetAsUsed()
    {
        simpleAnimator2D.SetAnimation(2);
        Destroy(GetComponent<Interactable>());
        Destroy(this);
    }

    //invoked by Interactable Script.
    public void OpenChest()
    {
        StartCoroutine(nameof(OpenChestSteps));
    }

    public IEnumerator OpenChestSteps()
    {
        FreezeMmanager.FreezeAll<PauseFreezer>(); //pause game
        simpleAnimator2D.SetAnimation(1); //open box

        yield return new WaitForSeconds(1.4f);//wait for box open to finish
        Vector3 player = FindFirstObjectByType<PlayerStateManager>().transform.position;
        GameObject heldAbovePlayer = Instantiate(new GameObject(), player + Vector3.up + Vector3.back*1, Quaternion.identity); //create sprite to be held up...
        heldAbovePlayer.AddComponent<SpriteRenderer>().sprite = chestTreasure.spriteInsideChest; //what sprite is it...

        ApplyChestsContents();

        yield return new WaitForSeconds(2f);
        Destroy(heldAbovePlayer);
        FreezeMmanager.UnfreezeAll<PauseFreezer>();
        SetAsUsed();
    }

    void ApplyChestsContents()
    {
        int indexOrQuantity = chestTreasure.indexOrQuantity; // saving on typing

        SaveFile save = SaveManager.GetSave();

        switch (chestTreasure.giveWhat)
        {
            case ChestTreasureList.GiveWhat.Nothing:
                break;

            case ChestTreasureList.GiveWhat.KeySelectableItem:
                if(indexOrQuantity >= save.ObtainedKeyItems.Length)
                {
                    Debug.LogError($"Key Selectable Item {indexOrQuantity} is not in range! (0,{save.ObtainedKeyItems.Length})");
                    break;
                }
                save.ObtainedKeyItems[indexOrQuantity] = true;
                FindFirstObjectByType<InventoryKeyItemSelector>(FindObjectsInactive.Include).SelectionIndex = indexOrQuantity; //select new item as ease to find out what it does.
                break;

            case ChestTreasureList.GiveWhat.KeyUnselectableItem:
                if(indexOrQuantity >= save.ObtainedKeyUnselectableItems.Length)
                {
                    Debug.LogError($"Key UnSelectable Item {indexOrQuantity} is not in range! (0,{save.ObtainedKeyUnselectableItems.Length})");
                    break;
                }
                save.ObtainedKeyUnselectableItems[indexOrQuantity] = true;
                break;

            case ChestTreasureList.GiveWhat.ConsumableItem:
                if(indexOrQuantity >= itemList.ConsumableItems.Length)
                {
                    Debug.LogError($"Key UnSelectable Item {indexOrQuantity} is not in range! (0,{itemList.ConsumableItems.Length})");
                    break;
                }
                SaveManager.AddConsumableItem(indexOrQuantity); //used as index.
                break;

            case ChestTreasureList.GiveWhat.Money:
                save.Money += indexOrQuantity;
                break;

            case ChestTreasureList.GiveWhat.Furniture:
                if(indexOrQuantity >= save.HouseItems.Length)
                {
                    Debug.LogError($"Furniture Item {indexOrQuantity} is not in range! (0,{save.HouseItems.Length})");
                    break;
                }
                save.HouseItems[indexOrQuantity] = true;
                break;

            case ChestTreasureList.GiveWhat.Sword:
                if(indexOrQuantity >= save.Swords.Length)
                {
                    Debug.LogError($"Sword {indexOrQuantity} is not in range! (0,{save.Swords.Length})");
                    break;
                }
                save.Swords[indexOrQuantity] = true;
                save.CurrentSword = indexOrQuantity;
                break;

            case ChestTreasureList.GiveWhat.Shield:
                if(indexOrQuantity >= save.Shields.Length)
                {
                    Debug.LogError($"Shield {indexOrQuantity} is not in range! (0,{save.Shields.Length})");
                    break;
                }
                save.Shields[indexOrQuantity] = true;
                save.CurrentShield = indexOrQuantity;
                break;

            case ChestTreasureList.GiveWhat.Boomerang:
                if(indexOrQuantity >= save.Boomerangs.Length)
                {
                    Debug.LogError($"Boomerang {indexOrQuantity} is not in range! (0,{save.Boomerangs.Length})");
                    break;
                }
                save.Boomerangs[indexOrQuantity] = true;
                save.CurrentBoomerang = indexOrQuantity;
                break;

            case ChestTreasureList.GiveWhat.Armor:
                if(indexOrQuantity >= save.Armors.Length)
                {
                    Debug.LogError($"Armor {indexOrQuantity} is not in range! (0,{save.Armors.Length})");
                    break;
                }
                save.Armors[indexOrQuantity] = true;
                save.CurrentArmor = indexOrQuantity;
                break;
            
        }
    }
}
