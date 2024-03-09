using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int startingHealth; //enemies may be able to gain more health then they start out with.
    protected Vector2 knockBackDirection; //direction to be knocked back in.
    protected float knockBackTimer; //timer before being able to be knocked back again.
    protected float invulnTimer; //Resting break before being able to be hit again.
    protected bool alwaysInvulnerable;
    protected Rigidbody2D rb;
    protected Health myHealth;

    //Common particle effects to spawn in when things happen.
    [SerializeField] GameObject hurtParticle;
    [SerializeField] GameObject dustParticle;
    [SerializeField] GameObject createDestroyParticle;

    // disable ourselves if we are this far away. Preformance gains.
    const float DISABLE_DISTANCE = 20;

    public void Start()
    {
        // Initialize variables.
        rb = GetComponent<Rigidbody2D>();

        // Set starting health.
        // if we are permanantely invulnerable, dont (we dont need to)
        if (alwaysInvulnerable) return;

        if (TryGetComponent(out Health h))
        {
            myHealth = h;
            myHealth.currentHealth = startingHealth;
        }
    }

    // NOTE: Do not use for enemy AI. Use EnemyAIUpdate, which can be paused by being knocked back.
    private void Update()
    {
        // Decrement timers.
        Timer.DecrementTimer(ref invulnTimer);
        Timer.DecrementTimer(ref knockBackTimer);

        // Conditionally allow enemy's AI to run.
        if (knockBackTimer <= 0)
        {
            AIUpdate();
        }

        /* Disable ourselves if we are too far away.
         * Other objects can re-enable us if needed (like say a dungeon enemy spawner)
         */
        GameObject player = FindFirstObjectByType<PlayerStateManager>().gameObject;
        if (Vector2.Distance(player.transform.position, transform.position) > DISABLE_DISTANCE)
        {
            gameObject.SetActive(false);
        }
    }

    // This is the only update function that enemies should override.
    // It can be toggled on/off depending on if it is being knocked back.
    public abstract void AIUpdate();


    // When hit, apply damage and knockback.
    /* TODO: 
     *  - Apply knockback when attack is actually done (eg NOT when in a triple hit)
     *  - Allow disabling of both damage and knockback.
     */
    public virtual void OnHit(GameObject from)
    {
        OnTakeDamage();
        ApplyKnockBack(from.transform);
    }

    //TODO: add different amounts of damage.
    public virtual void OnTakeDamage()
    {
        if (alwaysInvulnerable) return;
        if (invulnTimer > 0) return;
        //Create Particles
        Instantiate(hurtParticle, transform.position, Get2DAngleFromHit());

        //Update Health
        myHealth.TakeDamage(1);

        //Make sure we cant take more damage for some time.
        invulnTimer = 0.5f;
        GetComponentInChildren<EnemyHealthBar>().ShowHealthBar();

        AIOnTakeDamage();
    }
    public abstract void AIOnTakeDamage();

    // Turn our hit vector2 into an angle on the z axis.
    // This is only used for visual particle effects.
    public Quaternion Get2DAngleFromHit()
    {
        /* We use the player's position because sometimes the knockback direction can be at a wonky angle from the sword's position,
         * So it would look better if the direction of the particles came from the two biggest visual objects: the player and enemy.
         */
        Vector2 particleDirection = (transform.position - FindFirstObjectByType<PlayerStateManager>().transform.position).normalized;

        // Math stuff I copied online. I added negatives to fix wrong direction issues.
        float angle = Mathf.Atan2(particleDirection.y, -particleDirection.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, -Vector3.forward);
    }

    //Applies knockback while facing in the direction we were hit from.
    public void ApplyKnockBack(Transform from)
    {
        if (knockBackTimer > 0) return;

        // This is which direction to move in after we were hit, opposite from where we were hit.
        knockBackDirection = (transform.position - from.position).normalized;

        // Some objects may not have a direction, but if they do, we face the character towards the hit source.
        if (TryGetComponent(out DirectionedObject direction))
        {
            direction.direction = Vector2Int.RoundToInt(-knockBackDirection); //negative because normally its the direction to be knocked back.
        }

        // Create dust at feet.
        Instantiate(
            dustParticle,
            transform.position + Vector3.down * 0.5f,
            Get2DAngleFromHit(), transform);

        // Set knockback timer.
        knockBackTimer = 0.2f;

        // Actually knock back.
        rb.AddForce(knockBackDirection * 3, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out SwordHitBox sb))
        {
            OnHit(sb.gameObject);
            return;
        }
    }


}