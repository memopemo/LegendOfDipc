using UnityEngine;

public class DungeonKey : MonoBehaviour
{
    int index;
    void Start()
    {
        index = GameObjectParser.GetIndexFromName(gameObject);
    }
    public void OnCollect()
    {
        SaveFile save = SaveManager.GetSave();
        save.dungeons[index / Dungeon.MAX_KEYS].KeyObtained[index % Dungeon.MAX_KEYS] = true;
    }
}
