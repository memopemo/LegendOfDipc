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
        PlayerHealth.Heal(health, FindAnyObjectByType<PlayerStateManager>());
        Destroy(gameObject);
    }
}
