using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Status : MonoBehaviour
{
    const float FIRE_LENGTH_TICKS = 4;
    const float POISON_LENGTH_TICKS = 4;
    const float STUN_LENGTH_TICKS = 8;
    public enum Effect {
        Fire,   //being on fire damages more often but goes out quicker.
        Poison, //being poisoned damages less often but is extended.
        Stun    //being stunned slows movement and occasionally stops. Is as long as 
    };
    Health health; //allowed to be null.
    Rigidbody2D rb;
    PlayerStateManager player; //may be null if we aren't the player!
    [NonSerialized] public float[] statusTicks;
    float[] statusLength = new float[]{FIRE_LENGTH_TICKS, POISON_LENGTH_TICKS, STUN_LENGTH_TICKS};
    public GameObject[] statusParticles;
    float ogMass;

    void Awake()
    {
        player = GetComponent<PlayerStateManager>();
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
        statusTicks = new float[3];
        ogMass = rb.drag;

    }
    void Start()
    {
        InvokeRepeating(nameof(FireTick), 0, 1);
        InvokeRepeating(nameof(PoisonTick),0, 1.5f);
        InvokeRepeating(nameof(StunTick), 0, 2f);
    }
    void FireTick()
    {
        if(statusTicks[(int)Effect.Fire] <= 0) return;
        DecrementStatus(Effect.Fire);
        CreateParticle(Effect.Fire);
        TakeDamage();
    }

    void PoisonTick()
    {
        if(statusTicks[(int)Effect.Poison] <= 0) return;
        DecrementStatus(Effect.Poison);
        CreateParticle(Effect.Poison);
        TakeDamage();
    }

    void StunTick()
    {
        if(statusTicks[(int)Effect.Stun] <= 0) return;
        rb.drag = 9999;
        DecrementStatus(Effect.Stun);
        CreateParticle(Effect.Stun);
        Invoke(nameof(RemoveStun), 0.5f);
    }

    public void SetStatus(Effect effect)
    {
        statusTicks[(int)effect] = statusLength[(int)effect];
    }
    public void ClearStatus(Effect effect)
    {
        statusTicks[(int)effect] = 0;
    }
    public void ClearAllStatus()
    {
        for (int i = 0; i < statusTicks.Length; i++)
        {
           statusTicks[i] = 0;
        }
    }
    void DecrementStatus(Effect effect)
    {
        statusTicks[(int)effect] -= 1;
    }
    void CreateParticle(Effect effect)
    {
        Instantiate(statusParticles[(int)effect], transform.position, transform.rotation);
    }
    void RemoveStun()
    {
        rb.drag = ogMass;
    }
    void TakeDamage()
    {
        if(player)
        {
            PlayerHealth.TakeDamage(1, player);
        }
        else if(health)
        {
            health.TakeDamage(1);
        }
    }
}
