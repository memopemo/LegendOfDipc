using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownShopItem : MonoBehaviour
{
    Vector2 direction;
    public int id = 1;
    // Start is called before the first frame update
    void Start()
    {
        direction = new Vector2(Random.Range(-1f, 1f), -1).normalized * 10; //-45 to 45 degrees down
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)direction * Time.deltaTime; //move in direction over time.
        direction *= 0.95f; //slow down over time
    }
    public void Collect()
    {
        SaveManager.AddConsumableItem(id);
    }
}
