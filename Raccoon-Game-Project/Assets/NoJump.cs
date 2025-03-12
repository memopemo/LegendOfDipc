using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoJump : MonoBehaviour
{
    Collider2D collider2D;
    PlayerStateManager player;
    // Start is called before the first frame update
    void Start()
    {
        collider2D = GetComponent<Collider2D>();
        player = FindFirstObjectByType<PlayerStateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        collider2D.isTrigger = player.height.height == 0;
    }
}
