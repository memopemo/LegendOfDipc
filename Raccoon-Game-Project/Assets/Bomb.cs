using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] float timeUntilBoomSecs;
    void Start()
    {
        PlayerStateManager player = FindFirstObjectByType<PlayerStateManager>();
        if(player)
        {
            GetComponent<Rigidbody2D>().velocity = player.directionedObject.direction;
        }
        
    }
    void Update()
    {
        timeUntilBoomSecs -= Time.deltaTime;
        if(timeUntilBoomSecs <= 0)
        {
            GetComponent<PoofDestroy>().Poof();
            Destroy(gameObject);
        }
    }
}
