using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldPlayerItem : MonoBehaviour
{
    [NonSerialized] public PlayerStateManager player;
    [NonSerialized] public Vector2Int direction;
    // Start is called before the first frame update
    protected void Start()
    {
        player = FindFirstObjectByType<PlayerStateManager>();
        transform.parent = player.transform;
        player.SwitchState(new HeldItemPlayerState(this));
        direction = player.directionedObject.direction;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(!Input.GetButton("Fire3"))
        {
            (player.currentPlayerState as HeldItemPlayerState).ExitCanceled(player);
            Destroy(gameObject);
        }
    }
}
