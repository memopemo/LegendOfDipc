using UnityEngine;

public class StoryKey : MonoBehaviour
{
    int index;
    PlayerStateManager player;

    // Start is called before the first frame update
    void Start()
    {

        index = Common.GetIndexFromName(gameObject);
        if (SaveManager.GetSave().DemonKeys[index])
        {
            Destroy(gameObject);
        }
        player = FindFirstObjectByType<PlayerStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player) return;
        if (Vector2.Distance(player.transform.position, transform.position) <= 1)
        {
            SaveManager.GetSave().DemonKeys[index] = true;
            Destroy(gameObject);
        }
        GetComponent<Heightable>().height = (Mathf.Sin(Time.time * 4) / 4) + 0.5f;
    }

}
