
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

class CommonPlayerState
{
    public const float DEFAULT_SPEED = 6;
    public static void MovePlayerRaw(PlayerStateManager manager, float speed) 
    {
        manager.rigidBody.velocity = manager.rawInput * (speed + manager.additionalSpeed);
    }
    public static void MovePlayerSmooth(PlayerStateManager manager, float speed)
    {
        manager.rigidBody.AddForce(manager.rawInput * (speed+manager.additionalSpeed) * Time.deltaTime * 60);
    }

    public static void UpdateDirection(PlayerStateManager manager)
    {
        //Debug.Log(manager.directionedObject.direction);
        //TODO: maybe add anti-stick-flick, doesnt seem to be a problem rn with normal gameplay, 
        // only when intentionally flicking does it occur.
        // or add an option to movement when stick flicking.
        if (manager.rawInput != Vector2.zero)
        {
            manager.directionedObject.direction = Vector2Int.RoundToInt(manager.rawInput.normalized);
        }
        
    }
    public static float GetJumpHeight()
    {
        //TODO: change jump height based on what we have. 
        if (true)
        {
            return 2;
        }
        //else return JUMP_HEIGHT_NORMAL;
    }
    // Checks if a collider is in the way of anything.
    // ONLY READ OUTPUT IF YOU ARE SEARCHING FOR A TYPE.
    public static bool ColliderInDirection(PlayerStateManager manager, out GameObject go, bool useTriggers = false)
    {
        if (manager.directionedObject.direction.x != 0 && manager.directionedObject.direction.y != 0)
        {
            Debug.Log("Direction Is Diagonal.");
            go = null;
            return false; //Not allowed to check diagonally.
        }
        ContactFilter2D contactFilter = new ContactFilter2D() { layerMask = LayerMask.NameToLayer("Default"), useTriggers = useTriggers,};
        List<RaycastHit2D> results = new();
        /* LONG-WINDED EXPLANATION:
         * The box must be a little smaller than 1 so that we cant touch any walls to the side of our check. Only infront.
         * 
         * we start from a little bit in the direction (half) of the player so we dont hit anything behind us. 
         * Our circle hitbox is slightly smaller than a square to allow us to squeeze into 1 block gaps, but this makes it so that our 0.95 sized block still hits anything behind us.
         * This is because the player's hitbox is (by design) a little smaller than a 1 unit box, so a 1 unit box will overlap if we check while touching the sides or back.
         * Thats why we start a little bit in the players direction.
         * 
         * We go in the players direction (as a box*cast*) so that we hit any wall in our path. 
         * Walls in our game are litterally lines and not solid on the inside, so we must raycast into them.
         * Placing a box inside the wall will not work, as our box is too small (by design) to hit anything.
         * 
         */
        int _ = Physics2D.BoxCast(manager.transform.position+(Vector3)(Vector2)manager.directionedObject.direction*0.5f, Vector2.one*0.51f, 0, manager.directionedObject.direction, contactFilter, results, 0.5f);
        foreach (RaycastHit2D hit in results)
        {
            if (hit.collider.gameObject == manager.gameObject) continue;
            else 
            {
                go = hit.collider.gameObject;
                Debug.DrawLine(manager.transform.position+(Vector3)(Vector2)manager.directionedObject.direction*0.5f, hit.point, Color.red, 1);
                return true;
            }
        }
        go = null;
        return false;
    }

    //Checks if a collider exists in the area where the
    public static bool ColliderAt(PlayerStateManager manager, Vector2Int offsetDirection, out GameObject go, GameObject ignore, bool useTriggers = false) 
    {
        if(offsetDirection.x != 0 && offsetDirection.y != 0) 
        {
            go = null;
            return false;
        }
        ContactFilter2D contactFilter = new ContactFilter2D() { layerMask = LayerMask.NameToLayer("Default"), useTriggers = useTriggers };
        List<RaycastHit2D> results = new();
        Vector3 position = manager.transform.position;

        /* //Snap to grid
         position += (Vector3)Vector2.one * 0.5f;
         position = new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
         position -= (Vector3)Vector2.one * 0.5f;*/

        //-.01f so that we dont check 3 tiles forward. we only want 2
        //0.95 because we dont want to hit the sides

        
        int _ = Physics2D.BoxCast(position, Vector2.one*0.95f, 0, offsetDirection, contactFilter, results, offsetDirection.magnitude-0.2f);
        foreach (RaycastHit2D hit in results)
        {
            if (hit.collider.gameObject == manager.gameObject) continue;
            if (hit.collider.gameObject == ignore) continue;
            else
            {
                go = hit.collider.gameObject;
                Debug.DrawLine(position, hit.point, Color.red, 1);
                return true;
            }
        }
        go = null;
        return false;
    }

    public static void CheckWater(PlayerStateManager manager)
    {
        ContactFilter2D contactFilter = new()
        {
            layerMask = LayerMask.NameToLayer("Default"),
            useTriggers = true,
            minDepth = manager.transform.position.z,
            maxDepth = manager.transform.position.z + 3
            
        };
        Physics2D.queriesHitTriggers = true; //we want to check triggers.
        BoxCastFind<Water>(manager.transform.position + (Vector3.down * 0.2f), Vector2.one * 0.01f, (water) => manager.SwitchState(new SwimPlayerState()), contactFilter);
        Physics2D.queriesHitTriggers = false;
    }

    public static bool BoxCastFind<T>( Vector2 origin, Vector2 size, Action<T> onFind, ContactFilter2D contactFilter = new())
    {
        List<RaycastHit2D> results = new();
        int _ = Physics2D.BoxCast(origin, size, 0, Vector2.zero, contactFilter, results, 0);
        foreach (RaycastHit2D hit in results)
        {
            if (hit.collider.TryGetComponent(out T foundT))
            {
                onFind(foundT);
                return true;
            }
        }
        return false;
    }
}