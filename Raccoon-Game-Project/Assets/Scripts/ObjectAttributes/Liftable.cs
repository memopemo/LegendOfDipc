using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Grabbable), typeof(DirectionedObject), typeof(Heightable))]
public class Liftable : MonoBehaviour
{
    public bool isThrown;
    [NonSerialized] public Collider2D colider;
    [NonSerialized] public Heightable heightable;
    DirectionedObject direction;
    float downVelocity = -4f;
    CollisionCheck collisionCheck;
    // Start is called before the first frame update
    void Start()
    {
        colider = GetComponent<Collider2D>();
        direction = GetComponent<DirectionedObject>();
        heightable = GetComponent<Heightable>();
        collisionCheck = new(GetComponent<Collider2D>());
        collisionCheck.SetBoxSize(0.75f);
    }

    // Update is called once per frame
    void Update()
    {
        if (direction.direction != Vector2Int.zero)
        {
            //Decrease down velocity
            downVelocity += Time.deltaTime * 35;

            //set height
            heightable.height -= downVelocity * Time.deltaTime;
            //transform.position += Vector3.down * downVelocity * Time.deltaTime;
            transform.position += 11 * Time.deltaTime * (Vector3)(Vector2)direction.direction;
            if (heightable.height <= 0)
            {
                Break();
            }
            else
            {
                collisionCheck.EvaluateAnythingBut<PlayerStateManager>((_) => Break());
            }
        }

    }
    public void Break()
    {
        GetComponent<PoofDestroy>().Poof();
        Destroy(gameObject);
    }

    public void OnLifted()
    {

    }
    public void OnSetDown()
    {
        GetComponent<PoofDestroy>().Poof();
    }

    public void OnThrown(Vector2Int d)
    {
        direction.direction = d;
    }
}
