using System.Linq;
using UnityEngine;

public class IceBlock : MonoBehaviour
{
    [SerializeField] AnimationCurve reactionJump;
    float time;
    Heightable heightable;
    DirectionedObject directionedObject;
    Rigidbody2D rb;
    DamagesEnemy damagesEnemy;
    PoofDestroy poofDestroy;
    ParticleMaker particleMaker;
    const int SPEED = 10;
    const int BLOCK_POOF_PARTICLE = 0;
    const int BREAK_DESTROY_PARTICLE = 1;
    // Start is called before the first frame update
    void Start()
    {
        transform.position += (Vector3)(Vector2)FindFirstObjectByType<PlayerStateManager>().directionedObject.direction * 0.5f;
        transform.GetChild(0).SetParent(null, true);
        heightable = GetComponent<Heightable>();
        directionedObject = GetComponent<DirectionedObject>();
        rb = GetComponent<Rigidbody2D>();
        poofDestroy = GetComponent<PoofDestroy>();
        damagesEnemy = GetComponent<DamagesEnemy>();
        particleMaker = GetComponent<ParticleMaker>();
        directionedObject.direction = Vector2Int.zero;

    }

    // Update is called once per frame
    void Update()
    {
        if (time < reactionJump.keys.Last().time && time + Time.deltaTime > reactionJump.keys.Last().time)
        {
            particleMaker.CreateParticle(BLOCK_POOF_PARTICLE);
        }
        time += Time.deltaTime;
        heightable.height = reactionJump.Evaluate(time);//starting jump.
        rb.linearVelocity = directionedObject.direction * SPEED; //only move if direction is set. 

        damagesEnemy.enabled = kickable(); //only can hurt enemies if kicked.

    }
    bool kickable() => time >= reactionJump.keys.Last().time;
    void OnCollisionStay2D(Collision2D collision)
    {
        if (directionedObject.direction == Vector2Int.zero) //waiting on a kick
        {
            if (kickable() &&
                collision.collider.TryGetComponent(out PlayerStateManager player))
            {
                poofDestroy.Poof();
                directionedObject.direction = Vector2Int.RoundToInt(transform.position - player.transform.position); //kick 
            }
        }
        else
        {
            if (collision.collider.TryGetComponent(out Hittable hittable))
            {
                hittable.OnHit(damagesEnemy);
            }
            else if (collision.collider.TryGetComponent(out PlayerStateManager player))
            {
                return;
            }
            else
            {
                particleMaker.CreateParticle(BREAK_DESTROY_PARTICLE);
                Destroy(gameObject);
            }
        }

    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out Fire _))
        {
            //melt.
            poofDestroy.PoofAndDestroy();
        }
    }
}

