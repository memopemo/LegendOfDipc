using System.Collections;
using UnityEngine;
public class ImportantCollectable : MonoBehaviour
{
    public enum Type { HeartContainer, StoryKey, Pendant, ToiletPaper }
    [SerializeField] Type type;
    int index;
    PlayerStateManager player;
    SaveFile save;
    void Start()
    {
        index = GameObjectParser.GetIndexFromName(gameObject);
        save = SaveManager.GetSave();

        //if nothing happens OR errors, then it will stay deactivated.
        gameObject.SetActive(false);

        switch (type)
        {
            case Type.HeartContainer:
                if (!save.HeartContainersCollected[index])
                {
                    gameObject.SetActive(true);
                }
                break;
            case Type.StoryKey:
                if (!save.DemonKeys[index])
                {
                    gameObject.SetActive(true);
                }
                break;
            case Type.Pendant:
                if (!save.Pendants[index])
                {
                    gameObject.SetActive(true);
                }
                break;
            case Type.ToiletPaper:
                gameObject.SetActive(true);
                break;
        }
    }

    public void OnCollect()
    {
        GetComponent<Collectable>().enabled = false;
        StartCoroutine(nameof(CollectSequence));
    }

    public IEnumerator CollectSequence()
    {
        FreezeManager.FreezeAll<CutSceneFreezer>();
        PlayerStateManager player = FindFirstObjectByType<PlayerStateManager>();
        player.SetAnimation(36);
        player.directionedObject.direction = Vector2Int.down;
        transform.position = player.transform.position;
        GetComponent<MagicHover>().distanceOffGround = 1.25f;
        yield return new WaitForSeconds(2);
        FreezeManager.UnfreezeAll<CutSceneFreezer>();
        switch (type)
        {
            case Type.HeartContainer:
                save.HeartContainersCollected[index] = true;
                PlayerHealth.Heal(100, player);
                break;
            case Type.StoryKey:
                save.DemonKeys[index] = true;
                break;
            case Type.Pendant:
                save.Pendants[index] = true;
                break;
            case Type.ToiletPaper:
                save.ToiletPaperRolls++;
                break;
        }
        GetComponent<PoofDestroy>().PoofAndDestroy();

    }
}
