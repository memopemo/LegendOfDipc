using System.Collections;
using UnityEngine;
public class StoryKey : MonoBehaviour
{
    [SerializeField] Exit exitOfDungeon;
    
    void Start()
    {
        gameObject.SetActive(!SaveManager.GetSave().DemonKeys[GameObjectParser.GetIndexFromName(gameObject)]);
    }

    public void OnCollect()
    {
        GetComponent<Collectable>().enabled = false;
        FreezeManager.FreezeAll<CutSceneFreezer>();
        PlayerStateManager player = FindFirstObjectByType<PlayerStateManager>();
        player.SetAnimation(36);
        player.directionedObject.direction = Vector2Int.down;
        transform.position = player.transform.position;
        GetComponent<MagicHover>().distanceOffGround = 1.25f;
        SaveManager.GetSave().DemonKeys[GameObjectParser.GetIndexFromName(gameObject)] = true;
        StartCoroutine(nameof(CollectSequence));
    }

    public IEnumerator CollectSequence()
    {
        yield return new WaitForSeconds(2);
        exitOfDungeon.UseExit();
    }
}

