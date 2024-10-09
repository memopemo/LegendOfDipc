
using UnityEngine;
class Knockbackable : MonoBehaviour
{
    Vector2 knockBackDirection; //direction to be knocked back in.
    float knockBackTimer; //timer before being able to be knocked back again.
    [SerializeField] GameObject dustParticle;
    Rigidbody2D rb;
    [SerializeField] MonoBehaviour[] disabledOnKnockBack;
    bool componentsEnabled;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Timer.DecrementTimer(ref knockBackTimer);
        if(!componentsEnabled && knockBackTimer <= 0)
        {
            ReenableComponents();
        }
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
            Rotation.Get2DAngleFromPoints(transform.position, from.parent ? from.parent.position: from.position), transform);

        // Set knockback timer.
        knockBackTimer = 0.2f;

        // Actually knock back.
        rb.AddForce(knockBackDirection * 3, ForceMode2D.Impulse);

        foreach (var item in disabledOnKnockBack)
        {
            item.enabled = false;
        }
        componentsEnabled = false;
    }
    public void ReenableComponents()
    {
        foreach (var item in disabledOnKnockBack)
        {
            item.enabled = true;
        }
        componentsEnabled = true;
    }
}