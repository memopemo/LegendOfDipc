using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class CollisionCheck
{
    Vector2 relativePosition; //relative to the calling object, eg 1 unit up from me = V2(0,1)
    Vector2 relativeDragPosition; //relative to the relative position, so "and go 3 units this way" = V2(3,0)
    Vector2 boxSize;
    float radius;
    bool useTriggers;
    ContactFilter2D contactFilter;
    public enum CollisionType { Point, Line, SingleBox, DraggedBox, Circle, DraggedCircle }
    CollisionType collisionType;
    Collider2D origin;
    bool drawDebug;

    public CollisionCheck(Collider2D og)
    {
        relativePosition = Vector2.zero;
        relativeDragPosition = Vector2.zero;
        boxSize = Vector2.one;
        radius = 1;
        contactFilter = new()
        {
            layerMask = LayerMask.NameToLayer("Default")
        };
        useTriggers = false;
        origin = og;
    }

    public CollisionCheck SetRelativePosition(Vector2 v)
    {
        relativePosition = v;
        return this;
    }
    public CollisionCheck MoveFromDirection(float amount, DirectionedObject directionedObject)
    {
        relativePosition = (Vector2)directionedObject.direction * amount;
        return this;
    }
    public CollisionCheck SetDragPosition(Vector2 v)
    {
        relativeDragPosition = v;
        return this;
    }
    public CollisionCheck SetDragPositionFromDirection(float amount, DirectionedObject directionedObject)
    {
        relativeDragPosition = (Vector2)directionedObject.direction * amount;
        return this;
    }
    public CollisionCheck SetBoxSize(Vector2 size)
    {
        boxSize = size;
        return this;
    }
    public CollisionCheck SetBoxSize(float size)
    {
        boxSize = Vector2.one * size;
        return this;
    }
    public CollisionCheck SetCircleRadius(float n)
    {
        radius = n;
        return this;
    }
    public CollisionCheck SetZRange(float from, float to)
    {
        contactFilter.minDepth = from;
        contactFilter.maxDepth = to;
        return this;
    }
    public CollisionCheck SetCollisionLayer(int layer)
    {
        contactFilter.layerMask = layer;
        return this;
    }
    public CollisionCheck SetFindTriggers(bool canFindTriggers)
    {
        useTriggers = canFindTriggers;
        contactFilter.useTriggers = canFindTriggers;
        return this;
    }
    public CollisionCheck SetType(CollisionType type)
    {
        collisionType = type;
        return this;
    }
    public CollisionCheck SetDebug(bool b)
    {
        drawDebug = b;
        return this;
    }
    public bool Evaluate<Find>(Action<Find> onFound)
    {
        foreach (RaycastHit2D hit in Cast())
        {
            if (hit.collider == origin) continue;
            if (hit.collider.TryGetComponent(out Find found))
            {
                onFound(found);
                return true;
            }
        }
        return false;
    }
    public bool Evaluate<Find>(Action<Find> onFound, Predicate<Find> predicate)
    {
        foreach (RaycastHit2D hit in Cast())
        {
            if (hit.collider == origin) continue;
            if (hit.collider.TryGetComponent(out Find found))
            {
                if (!predicate(found)) continue;
                onFound(found);
                return true;
            }
        }
        return false;
    }
    public bool EvaluateAnythingBut<Exclude>(Action<Collider2D> onFound)
    {
        foreach (RaycastHit2D hit in Cast())
        {
            if (hit.collider == origin) continue;
            if (hit.collider.TryGetComponent(out Exclude _)) continue;
            onFound(hit.collider);
            return true;
        }
        return false;
    }

    public bool EvaluateAnything(Action<Collider2D> onFound)
    {
        foreach (RaycastHit2D hit in Cast())
        {
            if (hit.collider == origin) continue;
            onFound(hit.collider);
            return true;
        }
        return false;
    }

    public static bool EvaluateTouching<Include>(Rigidbody2D rb, out Include found)
    {
        // look through max of 2 objects we are contacting with.
        ContactPoint2D[] contactPoints = new ContactPoint2D[2];
        rb.GetContacts(contactPoints);

        foreach (var contact in contactPoints)
        {
            if (!contact.collider) continue;
            if (contact.collider.TryGetComponent(out Include thisT))
            {
                found = thisT;
                return true;
            }
        }
        found = default;
        return false;
    }

    List<RaycastHit2D> Cast()
    {
        Vector2 fromPosition = (Vector2)origin.transform.position + relativePosition;
        Vector2 toPosition = fromPosition + relativeDragPosition;
        float distance = Vector2.Distance(fromPosition, toPosition);
        Vector2 direction = (toPosition - fromPosition).normalized;
        List<RaycastHit2D> results = new();
        if (useTriggers)
        {
            Physics2D.queriesHitTriggers = true;
        }
        switch (collisionType)
        {
            case CollisionType.Point:
                if (drawDebug)
                {
                    Debug.DrawLine(fromPosition + Vector2.up / 4, fromPosition + Vector2.down / 4, Color.red);
                    Debug.DrawLine(fromPosition + Vector2.left / 4, fromPosition + Vector2.right / 4, Color.red);
                    Debug.DrawLine(fromPosition + (Vector2.up + Vector2.right) / 4, fromPosition + (Vector2.down + Vector2.left) / 4, Color.red);
                    Debug.DrawLine(fromPosition + (Vector2.down + Vector2.right) / 4, fromPosition + (Vector2.up + Vector2.left) / 4, Color.red);
                }
                Physics2D.Linecast(fromPosition, fromPosition, contactFilter, results);
                break;
            case CollisionType.Line:
                if (drawDebug) { Debug.DrawLine(fromPosition, toPosition); }
                Physics2D.Linecast(fromPosition, toPosition, contactFilter, results);
                break;
            case CollisionType.SingleBox:
                if (drawDebug)
                {
                    DebugDrawBox(fromPosition);
                }
                Physics2D.BoxCast(fromPosition, boxSize, 0, Vector2.zero, contactFilter, results, 0);
                break;
            case CollisionType.DraggedBox:
                if (drawDebug)
                {
                    DebugDrawBox(fromPosition);
                    DebugDrawBox(toPosition);
                }
                Physics2D.BoxCast(fromPosition, boxSize, 0, direction, contactFilter, results, distance);
                break;
            case CollisionType.Circle:
                if (drawDebug)
                {
                    DebugDrawCircle(fromPosition);
                }
                Physics2D.CircleCast(fromPosition, radius, Vector2.zero, contactFilter, results, 0);
                break;
            case CollisionType.DraggedCircle:
                if (drawDebug)
                {
                    DebugDrawCircle(fromPosition);
                    DebugDrawCircle(toPosition);
                }
                Physics2D.CircleCast(fromPosition, radius, direction, contactFilter, results, distance);
                break;
        }
        Physics2D.queriesHitTriggers = false;
        return results;
    }
    void DebugDrawBox(Vector2 pos)
    {
        Debug.DrawLine(pos + new Vector2(boxSize.x / 2, boxSize.y / 2), pos + new Vector2(boxSize.x / 2, -boxSize.y / 2), Color.red); //Right
        Debug.DrawLine(pos + new Vector2(boxSize.x / 2, boxSize.y / 2), pos + new Vector2(-boxSize.x / 2, boxSize.y / 2), Color.red); //Top
        Debug.DrawLine(pos + new Vector2(-boxSize.x / 2, boxSize.y / 2), pos + new Vector2(-boxSize.x / 2, -boxSize.y / 2), Color.red); //Left
        Debug.DrawLine(pos + new Vector2(-boxSize.x / 2, -boxSize.y / 2), pos + new Vector2(boxSize.x / 2, -boxSize.y / 2), Color.red); //Bottom
    }
    void DebugDrawCircle(Vector2 pos)
    {
        for (int i = 0; i < 16; i++)
        {
            Debug.DrawLine(getPoint(pos, i), getPoint(pos, i + 1), Color.red);
        }
        Vector2 getPoint(Vector2 pos, int i) => pos + new Vector2(getX(i), getY(i)) * radius;
        float getX(int i) => Mathf.Sin(i * Mathf.PI / 8);
        float getY(int i) => Mathf.Cos(i * Mathf.PI / 8);
    }


}