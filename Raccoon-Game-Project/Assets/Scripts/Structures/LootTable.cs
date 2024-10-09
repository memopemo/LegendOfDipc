using UnityEngine;

namespace AYellowpaper.SerializedCollections
{
    [CreateAssetMenu(fileName = "EnemyLootTable", menuName = "EnemyLootTable", order = 2)]
    public class LootTable : ScriptableObject
    {
        [SerializedDictionary("GameObject To Spawn", "Percent Chance")]
        SerializedDictionary<GameObject, float> lootTable;
    }
}

