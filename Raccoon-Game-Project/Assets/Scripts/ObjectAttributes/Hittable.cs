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
    Transform tempFrom;
    const float TRIPLE_HIT_FAIL_TIME = 0.4f;

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

        //dont let trapp
        if (!DemonBuffs.HasBuff(DemonBuffs.DemonBuff.Trap) && from.knockBack && knockbackable)
        {
            CancelInvoke();
            knockbackable.ApplyKnockBack(from.transform);
        }
        else if (knockbackable)
        {
            //trapped, but will eventually get out.
            tempFrom = from.transform;
            if (DemonBuffs.HasBuff(DemonBuffs.DemonBuff.Trap))
            {
                if (!IsInvoking())
                {
                    Invoke(nameof(OopsFailed), TRIPLE_HIT_FAIL_TIME + 1f);
                    StartCoroutine(nameof(HitStun));
                }
                return; //fall to this anyways
            }
            if (gameObject.activeSelf)
            {
                CancelInvoke();
                Invoke(nameof(OopsFailed), TRIPLE_HIT_FAIL_TIME);
                StopCoroutine(nameof(HitStun));
                StartCoroutine(nameof(HitStun));
            }
        }

    }
    public IEnumerator HitStun()
    {
        while (IsInvoking())
        {
            transform.position += Vector3.left / (Time.frameCount % 4 <= 1 ? 8 : -8);
            yield return null;
        }
    }
    public void OopsFailed() => knockbackable.ApplyKnockBack(tempFrom);

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
        invulnTimer = 0.06f;

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
