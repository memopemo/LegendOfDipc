using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pendant : MonoBehaviour
{
    int index;
    PlayerStateManager player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerStateManager>();
        index = GameObjectParser.GetIndexFromName(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(player.transform.position, transform.position) < 0.5f)
        {
            SaveManager.GetSave().Pendants[index] = true;
            GetComponent<PoofDestroy>().Poof();
            Destroy(gameObject);
        } 
    }
}
