using UnityEngine;
public class ImportantCollectable : MonoBehaviour
{
    public enum Type {HeartContainer, StoryKey, Pendant}
    [SerializeField] Type type;
    int index;
    PlayerStateManager player;
    SaveFile save;
    void Start()
    {
        index = GameObjectParser.GetIndexFromName(gameObject);
        player = FindAnyObjectByType<PlayerStateManager>();
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
        }
        GetComponent<NoiseMaker>().Play(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(!player)return;
        if (Vector2.Distance(player.transform.position, transform.position) <= 1)
        {
            switch (type)
            {
                case Type.HeartContainer:
                    save.HeartContainersCollected[index] = true;
                    PlayerHealth.Heal(2);
                    break;
                case Type.StoryKey:
                    save.DemonKeys[index] = true;
                    break;
                case Type.Pendant:
                    save.Pendants[index] = true;
                    break;
            }
            GetComponent<PoofDestroy>().Poof();
            Destroy(gameObject);
            GetComponent<NoiseMaker>().Play(1);
        }
    }
}
