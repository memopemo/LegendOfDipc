using System.Collections;
using System.Collections.Generic;
using Animator2D;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] float timeUntilBoomSecs;
    bool warningGiven;
    void Start()
    {
        PlayerStateManager player = FindFirstObjectByType<PlayerStateManager>();
        if(player)
        {
            GetComponent<Rigidbody2D>().linearVelocity = player.directionedObject.direction*5;
        }
        warningGiven = false;
        
    }
    void Update()
    {
        timeUntilBoomSecs -= Time.deltaTime;
        if(timeUntilBoomSecs <= 0)
        {
            GetComponent<PoofDestroy>().Poof();
            Destroy(gameObject);
        }
        if(!warningGiven && timeUntilBoomSecs <= 0.5f )
        {
            warningGiven = true;
            GetComponent<PalleteFlasher>().StartFlash();
        }
    }
}
