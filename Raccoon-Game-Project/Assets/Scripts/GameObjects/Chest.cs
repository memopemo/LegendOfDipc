using Animator2D;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

public class Chest : MonoBehaviour
{
    SimpleAnimator2D simpleAnimator2D;
    [SerializeField] ChestTreasureList chestTreasureList;
    [SerializeField] int indexIntoChestTreasureList;
    ChestTreasureList.ChestTreasure chestTreasure;
    [SerializeField] bool isImportant; //if important, player holds both hands up.
    [SerializeField] GameObject itemHeldUp;
    NoiseMaker noiseMaker;
    int dungeon = -1;
    const int WHAT_CHAR_INDEX_IS_AFTER_THE_STRING_DUNGEON = 7;
    void Start()
    {
        if (gameObject.scene.name.StartsWith("Dungeon"))
        {
            dungeon = int.Parse(gameObject.scene.name[7].ToString());
        }
        noiseMaker = GetComponent<NoiseMaker>();
        simpleAnimator2D = GetComponent<SimpleAnimator2D>();
        GetComponent<Interactable>().OnInteract.AddListener(OpenChest);
        SaveFile sf = SaveManager.GetSave();

        if (indexIntoChestTreasureList < 0 || indexIntoChestTreasureList >= chestTreasureList.chestTreasure.Length)
        {
            Debug.LogError($"index {indexIntoChestTreasureList} into Chest is not in range! (0, {chestTreasureList.chestTreasure.Length})");
            SetAsUsed();
            return;
        }
        if (InDungeon)
        {
            if (sf.dungeons[dungeon].ChestOpened[indexIntoChestTreasureList])
            {
                Debug.Log("Empty because dungeon treasure already obtained according to save file.");
                SetAsUsed();
            }
            ChestTreasureList.ChestTreasure ct = chestTreasureList.chestTreasure[indexIntoChestTreasureList];
            if (ct.giveWhat == ChestTreasureList.GiveWhat.Key && sf.dungeons[dungeon].KeyObtained[ct.indexOrQuantity])
            {
                Debug.Log("Empty because key loot is already obtained according to save file.");
                SetAsUsed();
            }
        }
        else
        {
            if (indexIntoChestTreasureList < 0 || indexIntoChestTreasureList >= sf.OverworldTreasure.Length)
            {
                Debug.LogError($"index {indexIntoChestTreasureList} into Overworld Save File Treasure is not in range! (0, {sf.OverworldTreasure.Length})");
                SetAsUsed();
                return;
            }
            if (sf.OverworldTreasure[indexIntoChestTreasureList])
            {
                Debug.Log("Empty because overworld treasure already obtained according to save file.");
                SetAsUsed();
                return;
            }
        }
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
        noiseMaker.Play(0); //play open sound
        SongPlayer songPlayer = FindAnyObjectByType<SongPlayer>();
        songPlayer?.StartFadeOut(0.75f);
        PlayerStateManager player = FindFirstObjectByType<PlayerStateManager>(); //get player reference for later.
        FreezeManager.FreezeAll<PauseFreezer>(); //pause game
        simpleAnimator2D.SetAnimation(1); //open box

        yield return new WaitForSeconds(0.75f);

        Vector2Int ogDirection = player.directionedObject.direction; //keep og direction after we are done.
        player.directionedObject.direction = Vector2Int.down;

        noiseMaker.Play(1, FindFirstObjectByType<Camera>().transform.position); //play "tadaaaaa!" sound

        yield return new WaitForSeconds(0.53f);
        player.directionedObject.direction = isImportant ? Vector2Int.down : Vector2Int.up; //set actual player's direction for types of importance of items.
        player.animator.SetAnimation(36); //play hold up item animation

        yield return new WaitForSeconds(0.17f); //wait for player animation to finish

        Vector3 playerPos = player.transform.position; //use tf
        GameObject heldAbovePlayer = Instantiate(itemHeldUp, playerPos + Vector3.up + Vector3.back * 1, Quaternion.identity); //create sprite to be held up...
        heldAbovePlayer.GetComponent<SpriteRenderer>().sprite = chestTreasure.spriteInsideChest; //what sprite is it...

        ApplyChestsContents();
        yield return new WaitForSeconds(1.5f);
        songPlayer?.StartFadeIn(1);

        yield return new WaitForSeconds(0.5f);

        player.directionedObject.direction = ogDirection; //set og position.
        Destroy(heldAbovePlayer); //get rid of spawned item.
        FreezeManager.UnfreezeAll<PauseFreezer>(); //return gameplay back to useable state
        SetAsUsed(); //done.

        if (InDungeon)
        {
            SaveManager.GetSave().dungeons[dungeon].ChestOpened[indexIntoChestTreasureList] = true;
        }
        else
        {
            SaveManager.GetSave().OverworldTreasure[indexIntoChestTreasureList] = true;
        }


    }

    private bool InDungeon => dungeon >= 0;

    void ApplyChestsContents()
    {
        int indexOrQuantity = chestTreasure.indexOrQuantity; // saving on typing

        SaveFile save = SaveManager.GetSave();
        Dungeon dungeonData = null;
        if (InDungeon)
        {
            dungeonData = save.dungeons[dungeon];
        }


        switch (chestTreasure.giveWhat)
        {
            case ChestTreasureList.GiveWhat.Nothing:
                break;

            case ChestTreasureList.GiveWhat.KeySelectableItem:
                save.ObtainedKeyItems[indexOrQuantity] = true;
                SelectedItem.KeyItem = indexOrQuantity; //select new item as ease to find out what it does.
                break;

            case ChestTreasureList.GiveWhat.KeyUnselectableItem:
                save.ObtainedKeyUnselectableItems[indexOrQuantity] = true;
                break;

            case ChestTreasureList.GiveWhat.ConsumableItem:
                SaveManager.AddConsumableItem(indexOrQuantity); //used as index.
                break;

            case ChestTreasureList.GiveWhat.Money:
                save.Money += indexOrQuantity;
                break;

            case ChestTreasureList.GiveWhat.Furniture:
                save.HouseItems[indexOrQuantity] = true;
                break;

            case ChestTreasureList.GiveWhat.Sword:
                save.Swords[indexOrQuantity] = true;
                save.CurrentSword = indexOrQuantity;
                break;

            case ChestTreasureList.GiveWhat.Shield:
                save.Shields[indexOrQuantity] = true;
                save.CurrentShield = indexOrQuantity;
                break;

            case ChestTreasureList.GiveWhat.Boomerang:
                save.Boomerangs[indexOrQuantity] = true;
                save.CurrentBoomerang = indexOrQuantity;
                break;

            case ChestTreasureList.GiveWhat.Armor:
                save.Armors[indexOrQuantity] = true;
                save.CurrentArmor = indexOrQuantity;
                break;
            case ChestTreasureList.GiveWhat.HeartContainer:
                save.HeartContainersCollected[indexOrQuantity] = true;
                PlayerHealth.Heal(2);
                break;
            case ChestTreasureList.GiveWhat.Key when InDungeon:
                dungeonData.KeyObtained[indexOrQuantity] = true;
                break;
            case ChestTreasureList.GiveWhat.SkeletonKey when InDungeon:
                dungeonData.SkeletonKeyObtained = true;
                break;
            case ChestTreasureList.GiveWhat.BossKey when InDungeon:
                dungeonData.BossKeyObtained = true;
                break;
        }
    }
}
