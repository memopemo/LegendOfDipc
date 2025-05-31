using Animator2D;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

public class Chest : MonoBehaviour
{
    SimpleAnimator2D simpleAnimator2D;
    [SerializeField] ChestTreasureList chestTreasureList;
    [SerializeField] int indexIntoChestTreasureList; //this is the treasure to give.
    Treasure chestTreasure;
    [SerializeField] bool isImportant; //if important, player holds both hands up.
    [SerializeField] GameObject itemHeldUp;
    NoiseMaker noiseMaker;
    SaveFile saveFile;
    void Start()
    {
        //chest index in name is used for telling the save file that this chest has been opened.
        if(Randomizer.isRandoActive)
        {
            indexIntoChestTreasureList = Randomizer.randomizedChestIndexMapping[indexIntoChestTreasureList];
        }
        noiseMaker = GetComponent<NoiseMaker>();
        simpleAnimator2D = GetComponent<SimpleAnimator2D>();
        GetComponent<Interactable>().OnInteract.AddListener(OpenChest);
        saveFile = SaveManager.GetSave();

        if (indexIntoChestTreasureList < 0 || indexIntoChestTreasureList >= chestTreasureList.chestTreasure.Length)
        {
            Debug.LogError($"index {indexIntoChestTreasureList} into Chest is not in range! (0, {chestTreasureList.chestTreasure.Length})");
            SetAsUsed();
            return;
        }
        else
        {
            if (indexIntoChestTreasureList < 0 || indexIntoChestTreasureList >= saveFile.ChestOpened.Length)
            {
                Debug.LogError($"index {indexIntoChestTreasureList} into Overworld Save File Treasure is not in range! (0, {saveFile.ChestOpened.Length})");
                SetAsUsed();
                return;
            }
            if (saveFile.ChestOpened[indexIntoChestTreasureList])
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

        saveFile.ChestOpened[indexIntoChestTreasureList] = true;
    }

    void ApplyChestsContents()
    {
        chestTreasure.OnRecieved();
        FindFirstObjectByType<PlayerStateManager>().UpdateColors(); //update colors if sword/cheild/player were changed.
    }
}
