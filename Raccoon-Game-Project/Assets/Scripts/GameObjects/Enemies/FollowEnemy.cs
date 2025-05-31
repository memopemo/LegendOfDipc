using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class FollowEnemy : MonoBehaviour
{
    PlayerStateManager player;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerStateManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.linearVelocity = (player.transform.position-transform.position).normalized;
        GetComponent<DirectionedObject>().direction = Vector2Int.RoundToInt(rb.linearVelocity);
    }
}
