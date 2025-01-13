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
        GetComponent<NoiseMaker>().Play(0);
    }

    public void OnCollect()
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
            case Type.ToiletPaper:
                save.ToiletPaperRolls++;
                break;
        }
        GetComponent<NoiseMaker>().Play(1);
    }
}
