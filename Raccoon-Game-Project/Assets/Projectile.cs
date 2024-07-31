using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
//do not put a collider on this object!
public class Projectile : MonoBehaviour
{
    public bool IsMagic;
    public UnityEvent OnHit;
    public bool isHitting;
    Vector2 boxSize;
    void Start()
    {
        boxSize = GetComponent<BoxCollider2D>().size;
    }
    void Update()
    {
        if(isHitting) return;
        
        Collider2D collider;
        collider = Physics2D.OverlapBox(transform.position, boxSize, 0);
        
        if(collider)
        {
            if(IsMagic && collider.TryGetComponent(out TilemapCollider2D _))
            {
                return;
            }
            OnHit.Invoke();
            isHitting = true;
        }
    }
    public void Die()
    {
        GetComponent<PoofDestroy>()?.Poof();
        Destroy(gameObject);
    }
}
