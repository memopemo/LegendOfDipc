using System.Collections;
using UnityEngine;

public class ImportantCollectable : MonoBehaviour
{
    public enum Type { HeartContainer, Pendant, ToiletPaper, BossKey, SkeletonKey }

    private const float RAISE_HEIGHT = 1.25f;
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
                if (save.HeartContainersCollected[index])
                    return;
                break;
            case Type.Pendant:
                if (save.Pendants[index])
                    return;
                break;
            case Type.BossKey:
                if (save.dungeons[index].BossKeyObtained)
                    return;
                break;
            case Type.SkeletonKey:
                if (save.dungeons[index].SkeletonKeyObtained)
                    return;
                break;
            default:
                break;
        }
        gameObject.SetActive(true);
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
        if(TryGetComponent(out MagicHover magicHover))
        {
            magicHover.distanceOffGround = RAISE_HEIGHT;
        }
        else if(TryGetComponent(out Heightable heightable))
        {
            heightable.height = RAISE_HEIGHT;
        }
        
        yield return new WaitForSeconds(2);
        FreezeManager.UnfreezeAll<CutSceneFreezer>();
        switch (type)
        {
            case Type.HeartContainer:
                save.HeartContainersCollected[index] = true;
                PlayerHealth.Heal(100, player);
                break;
            case Type.Pendant:
                save.Pendants[index] = true;
                break;
            case Type.ToiletPaper:
                save.ToiletPaperRolls++;
                break;
            case Type.BossKey:
                save.dungeons[index].BossKeyObtained = true;
                break;
            case Type.SkeletonKey:
                save.dungeons[index].SkeletonKeyObtained = true;
                break;
        }
        GetComponent<PoofDestroy>().PoofAndDestroy();

    }
}
