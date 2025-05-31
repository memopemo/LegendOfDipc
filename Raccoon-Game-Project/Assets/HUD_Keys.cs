using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//assumes:
// self is boss key sprite
// children are key sprites
public class HUD_Keys : MonoBehaviour
{
    SaveFile saveFile;
    [SerializeField] Sprite skeletonKeySprite;

    void Start()
    {
        saveFile = SaveManager.GetSave();
    }


    void Update()
    {
        Dungeon dungeon = saveFile.dungeons[GameObjectParser.GetIndexFromString(gameObject.scene.name)];

        GetComponent<Image>().enabled = dungeon.BossKeyObtained;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        if(dungeon.SkeletonKeyObtained)
        {
            Transform firstKey = transform.GetChild(0);
            firstKey.GetComponent<Image>().sprite = skeletonKeySprite;
            firstKey.gameObject.SetActive(true);
            return;
        }

        int keyCount = dungeon.KeyObtained.Count((x)=>x);
        for (int i = 0; i < keyCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(keyCount > i);
        }
    }
}
