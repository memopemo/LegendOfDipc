using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(Rigidbody2D))]
public class Hittable : MonoBehaviour
{
    Health myHealth;
    float invulnTimer; //Resting break before being able to be hit again.
    bool alwaysInvulnerable;
    [SerializeField] GameObject hurtParticle;
    Rigidbody2D rb;
    Knockbackable knockbackable;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        knockbackable = GetComponent<Knockbackable>();
        myHealth = GetComponent<Health>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out DamagesEnemy hurtful))
        {
            OnHit(hurtful);
            return;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out DamagesEnemy hurtful))
        {

            return;
        }
    }
    public virtual void OnHit(DamagesEnemy from)
    {
        TakeDamage(from.amount, from.isBuffed);
        if (knockbackable)
        {
            knockbackable.ApplyKnockBack(from.transform);
        }
    }

    //TODO: add buff-induced status effects (poison, stun)
    public virtual void TakeDamage(int amount, bool isPlayer)
    {
        if (alwaysInvulnerable) return;
        if (invulnTimer > 0) return;
        //Create Particles
        /* We use the player's position because sometimes the knockback direction can be at a wonky angle from the sword's position,
         * So it would look better if the direction of the particles came from the two biggest visual objects: the player and enemy.
         */
        Instantiate(hurtParticle, transform.position, Rotation.Get2DAngleFromPoints(transform.position, FindFirstObjectByType<PlayerStateManager>().transform.position));

        if (myHealth)
        {
            //Update Health
            if (isPlayer)
            {
                myHealth.TakeDamage(HitCalculation.PlayerHurtEnemyAmount(amount));
            }
            else
            {
                //else, was environmental damage (spikes, infighting, bombs, etc.)
                myHealth.TakeDamage(amount);
            }
        }


        //Make sure we cant take more damage for some time.
        invulnTimer = 0.5f;

        EnemyHealthBar healthBar = GetComponentInChildren<EnemyHealthBar>();
        if (healthBar)
        {
            healthBar.ShowHealthBar();
        }
    }
    void Update()
    {
        Timer.DecrementTimer(ref invulnTimer);
    }


}
