using UnityEngine;

public class HouseItemFrame : MonoBehaviour
{
    int index;
    enum Type { Sword, Shield, Armor, Boomerang }
    [SerializeField] Type type;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //for some reason, and i have *no idea why*, interactables just become unscannable unless i do this.
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        SaveFile save = SaveManager.GetSave();
        //disable entirely if we have not gotten the sword
        bool shouldExist = false;
        index = GameObjectParser.GetIndexFromName(gameObject);
        switch (type)
        {
            case Type.Sword:
                if (save.Swords[index])
                {
                    shouldExist = true;
                }
                break;
            case Type.Shield:
                if (save.Shields[index])
                {
                    shouldExist = true;
                }
                break;
            case Type.Armor:
                if (save.Armors[index])
                {
                    shouldExist = true;
                }
                break;
            case Type.Boomerang:
                if (save.Boomerangs[index])
                {
                    shouldExist = true;
                }
                break;
        }
        gameObject.SetActive(shouldExist);
    }
    public void Update()
    {
        SaveFile save = SaveManager.GetSave();
        switch (type)
        {
            case Type.Sword:
                spriteRenderer.enabled = save.CurrentSword != index;
                break;
            case Type.Shield:
                spriteRenderer.enabled = save.CurrentShield != index;
                break;
            case Type.Armor:
                spriteRenderer.enabled = save.CurrentArmor != index;
                break;
            case Type.Boomerang:
                spriteRenderer.enabled = save.CurrentBoomerang != index;
                break;
        }
    }

    public void OnInteract()
    {
        SaveFile saveFile = SaveManager.GetSave();

        switch (type)
        {
            case Type.Sword:
                if (saveFile.CurrentSword == index)
                {
                    saveFile.CurrentSword = -1;
                    break;
                }
                saveFile.CurrentSword = index;
                break;
            case Type.Shield:
                if (saveFile.CurrentShield == index)
                {
                    saveFile.CurrentShield = -1;
                    break;
                }
                saveFile.CurrentShield = index;
                break;
            case Type.Armor:

                if (saveFile.CurrentArmor == index)
                {
                    saveFile.CurrentArmor = -1;
                    break;
                }
                saveFile.CurrentArmor = index;

                break;
            case Type.Boomerang:
                if (saveFile.CurrentBoomerang == index)
                {
                    saveFile.CurrentBoomerang = -1;
                    break;
                }
                saveFile.CurrentBoomerang = index;
                break;

        }
        FindFirstObjectByType<PlayerStateManager>().UpdateColors();
    }
}
