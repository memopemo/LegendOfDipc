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
    // Start is called before the first frame update
    void Start()
    {
        colider = GetComponent<Collider2D>();
        direction = GetComponent<DirectionedObject>();
        heightable = GetComponent<Heightable>();
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
                Destroy(gameObject, 0.02f);
            }
            else
            {
                ContactFilter2D contactFilter = new() { layerMask = LayerMask.NameToLayer("Default") };
                List<RaycastHit2D> results = new();
                int _ = Physics2D.BoxCast(transform.position, Vector2.one * 0.75f, 0, Vector2.zero, contactFilter, results);
                foreach (RaycastHit2D hit in results)
                {

                    if (hit.collider == colider) continue;
                    if (hit.collider.TryGetComponent(out PlayerStateManager _)) continue;
                    print(hit.collider.gameObject);
                    Destroy(gameObject, 0.01f);
                }
            }
        }

    }
    public void OnLifted()
    {

    }

    public void OnThrown(Vector2Int d)
    {
        direction.direction = d;
    }
}
