using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FireworkStar : MonoBehaviour
{
    Rigidbody2D rb;
    DirectionedObject direction;
    [SerializeField] GameObject trail;
    [SerializeField] GameObject startParticle;
    int frameCount;
    const int rate = 3;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        direction = GetComponent<DirectionedObject>();
        InvokeRepeating(nameof(MakeTrail), 0, 0.05f);
        RaycastHit2D[] inbetween = Physics2D.RaycastAll(transform.position, -direction.direction, 1f);
        GameObject player = FindFirstObjectByType<PlayerStateManager>().gameObject;
        foreach(var collision in inbetween)
        {
            if(collision.rigidbody.gameObject == gameObject) continue;
            if(collision.rigidbody.gameObject == player) continue;
            OnCollisionEnter2D();
        }
        Instantiate(startParticle, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = direction.direction * 30;
    }

    void OnCollisionEnter2D()
    {
        GetComponent<PoofDestroy>().Poof();
        Destroy(gameObject);
    }
    void MakeTrail()
    {
        Instantiate(trail, transform.position, Quaternion.identity);
    }
}
