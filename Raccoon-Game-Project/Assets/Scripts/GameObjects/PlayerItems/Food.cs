using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    // Start is called before the first frame update
    public int index;
    void Start()
    {
        string[][] paramsFile = CSVReader.GetCSV("FoodParams");
        int health = int.Parse(paramsFile[index + 1][1]);
        if (PlayerHealth.currentHealth < PlayerHealth.GetMaxHealth())
        {
            PlayerHealth.Heal(health, FindAnyObjectByType<PlayerStateManager>());
            //Make the player do an eating or drinking animation.
        }
        else
        {
            //deny consuming the object.
            SaveManager.GetSave().InventoryConsumableCount[SelectedItem.ConsumableItem] += 1;
            return;
        }
        Destroy(gameObject);
    }
}
