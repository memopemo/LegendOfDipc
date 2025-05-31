
using UnityEngine;
class Knockbackable : MonoBehaviour
{
    Vector2 knockBackDirection; //direction to be knocked back in.
    [SerializeField] GameObject dustParticle;
    Rigidbody2D rb;
    [SerializeField] MonoBehaviour[] disabledOnKnockBack;
    bool componentsEnabled = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    //Applies knockback while facing in the direction we were hit from.
    public void ApplyKnockBack(Vector2 from)
    {
        if (IsInvoking()) return;

        // This is which direction to move in after we were hit, opposite from where we were hit.
        knockBackDirection = ((Vector2)transform.position - from).normalized;

        SetComponents(false);

        // Some objects may not have a direction, but if they do, we face the character towards the hit source.
        if (TryGetComponent(out DirectionedObject direction))
        {
            direction.direction = Vector2Int.RoundToInt(-knockBackDirection); //negative because normally its the direction to be knocked back.
        }
        // Create dust at feet.
        Instantiate(
            dustParticle,
            transform.position + Vector3.down * 0.5f,
            Rotation.Get2DAngleFromPoints(transform.position, from), transform);

        // Set knockback timer.
        Invoke(nameof(Unknockback), 0.2f);

        // Actually knock back.
        rb.AddForce(knockBackDirection * 3, ForceMode2D.Impulse);


    }
    void Unknockback()
    {
        SetComponents(true);
    }
    public void SetComponents(bool On)
    {
        if (componentsEnabled == On) return;
        foreach (var item in disabledOnKnockBack)
        {
            item.enabled = On;
        }
        componentsEnabled = On;
    }
}