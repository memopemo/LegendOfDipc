using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class Projectile : MonoBehaviour
{
    public bool IsMagic;
    public UnityEvent<Vector2> OnHit;
    public bool isHitting;
    float boxSize;
    void Start()
    {
        boxSize = 0.2f;
        if(TryGetComponent(out CircleCollider2D cc))
        {
            boxSize = cc.radius;
        }
    }
    void FixedUpdate()
    {
        if(isHitting) return;
        
        
        if(TryGetComponent(out FineDirectionedObject moveStraight))
        {
            RaycastHit2D raycastHit = Physics2D.CircleCast(transform.position, boxSize, moveStraight.direction, moveStraight.direction.magnitude * Time.deltaTime);
            if(raycastHit.collider)
            {
                if(IsMagic && raycastHit.collider.gameObject.TryGetComponent(out TilemapCollider2D _))
                {
                    return;
                }
                OnHit.Invoke(raycastHit.normal);
                isHitting = true;
                return;
            }
        }
        Collider2D hit;
        hit = Physics2D.OverlapCircle(transform.position, boxSize);
        
        if(hit)
        {
            if(IsMagic && hit.gameObject.TryGetComponent(out TilemapCollider2D _))
            {
                return;
            }
            Vector2 normal = (hit.ClosestPoint(transform.position) - (Vector2)transform.position).normalized;
            print(normal);
            OnHit.Invoke(normal);
            isHitting = true;
        }
    }
    public void Die()
    {
        GetComponent<PoofDestroy>()?.Poof();
        Destroy(gameObject);
    }
}
