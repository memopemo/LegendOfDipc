using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldPlayerItem : MonoBehaviour
{
    [NonSerialized] public PlayerStateManager player;
    void Awake()
    {
        player = FindFirstObjectByType<PlayerStateManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = player.transform;
        player.SwitchState(new HeldItemPlayerState(this));
        GetComponent<DirectionedObject>().direction = player.directionedObject.direction;
    }

    // Update is called once per frame
    void Update()
    {
        if(!Input.GetButton("Fire3"))
        {
            (player.currentPlayerState as HeldItemPlayerState).ExitCanceled(player);
            Destroy(gameObject);
        }
    }
}
