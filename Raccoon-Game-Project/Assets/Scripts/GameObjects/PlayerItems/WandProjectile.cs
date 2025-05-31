using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandProjectile : MonoBehaviour
{
    private const int PJ_SPEED = 15;
    public bool isSuper;
    ParticleMaker particleMaker;
    Dictionary<Vector2Int, Vector2> initDirection =
    new()
    {
        {Vector2Int.up, Vector2.one},
        {Vector2Int.down, -Vector2.one},
        {Vector2Int.left, new Vector2(-1,1)},
        {Vector2Int.right, new Vector2(1,-1)},
    };

    // Start is called before the first frame update
    void Start()
    {
        if(FindObjectsByType<WandProjectile>(FindObjectsSortMode.None).Length == 4)
        {
            Destroy(gameObject);
            return;
        }
        FineDirectionedObject dir = GetComponent<FineDirectionedObject>();
        Vector2Int playerDir = FindFirstObjectByType<PlayerStateManager>().directionedObject.direction;
        if (isSuper)
        {
            initDirection.TryGetValue(playerDir, out Vector2 direction);
            dir.direction = direction;
        }
        else
        {
            dir.direction = playerDir;
        }
        dir.direction *= PJ_SPEED;
        Invoke(nameof(Die), 5);
        particleMaker = GetComponent<ParticleMaker>();
        InvokeRepeating(nameof(Trail),0f,0.1f);
        GetComponent<NoiseMaker>().Play(0);
        
    }
    void Die()
    {
        GetComponent<Projectile>().Die();
    }
    void Trail()
    {
        particleMaker.CreateParticle(0);
    }
}
