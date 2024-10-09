using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySnake : MonoBehaviour
{
    DirectionedObject direction;
    PlayerStateManager target;
    void Start()
    {
        direction = GetComponent<DirectionedObject>();
        target = FindFirstObjectByType<PlayerStateManager>();
    }
    public void Tick()
    {
        //Get next direction;
        Vector2Int nextDirection = Vector2Int.RoundToInt((target.transform.position - transform.position).normalized);
        if(!CanGoInDirection(nextDirection))
            nextDirection = Vector2Int.up;
        if(!CanGoInDirection(nextDirection))
            nextDirection = Vector2Int.right;
        if(!CanGoInDirection(nextDirection))
            nextDirection = Vector2Int.down;
        if(!CanGoInDirection(nextDirection))
            nextDirection = Vector2Int.left;
        if(!CanGoInDirection(nextDirection))
            return;
        direction.direction = nextDirection;
        transform.position += (Vector3)(Vector2)direction.direction;
    }
    bool CanGoInDirection(Vector2Int dir)
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll((Vector2)transform.position, Vector2.one*0.5f, 0, dir, 1);
        foreach (var hit in hits)
        {
            if(hit.collider.gameObject == gameObject) continue;
            if(hit.collider.gameObject == target.gameObject) continue;
            return false;
        }
        return true;
    }
}
