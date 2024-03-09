using System.Collections.Generic;
using UnityEngine;

// Move around, Jump, Sword, and Shield
public class DefaultPlayerState : IPlayerState
{
    const float DEFAULT_SPEED = 6f;

    const int ANIM_IDLE = 0;
    const int ANIM_RUN = 1;
    const int ANIM_JUMP = 3;
    const int ANIM_LAND = 3;
    float impliedPushingTimer;

    void IPlayerState.OnEnter(PlayerStateManager manager)
    {
    }

    void IPlayerState.OnLeave(PlayerStateManager manager)
    {
    }

    void IPlayerState.OnUpdate(PlayerStateManager manager)
    {
        CommonPlayerState.MovePlayerRaw(manager, DEFAULT_SPEED);
        CommonPlayerState.UpdateDirection(manager);

        // Check Jump
        if (Input.GetButtonDown("Jump"))
        {
            //Find a ledge top if we are jumping downwards.
            if (manager.inputY < 0)
            {
                ContactPoint2D[] contactPoints = new ContactPoint2D[2];
                manager.rigidBody.GetContacts(contactPoints);
                foreach (var contact in contactPoints)
                {
                    if (!contact.collider) continue;
                    if (contact.collider.gameObject.TryGetComponent(out LedgeTop ledge))
                    {
                        manager.animator.SetAnimation(ANIM_JUMP);
                        manager.SwitchState(new JumpingLedgePlayerState(0, ledge.bottom.transform.position.y, ledge.layersToDecrease));
                        manager.directionedObject.direction = Vector2Int.down;
                        //Debug.Log("Found A Top Ledge, Jumping Down.");
                        return;
                    }
                }
            }

            //Find a ledge side if we are jumping sideways.
            if (manager.inputX != 0 || manager.inputY > 0)
            {
                ContactPoint2D[] contactPoints = new ContactPoint2D[2];
                manager.rigidBody.GetContacts(contactPoints);
                foreach (var contact in contactPoints)
                {
                    if (!contact.collider) continue;
                    if (contact.collider.gameObject.TryGetComponent(out BackSideLedge ledge))
                    {
                        manager.animator.SetAnimation(ANIM_JUMP);
                        manager.SwitchState(new JumpingLedgePlayerState(-1, 0, ledge.layersToDrop));
                        return;
                    }
                }
            }


            //Find a ledge bottom if we are jumping up a ledge.
            if (manager.inputY > 0)
            {
                // look through max of 2 objects we are contacting with.
                ContactPoint2D[] contactPoints = new ContactPoint2D[2];
                manager.rigidBody.GetContacts(contactPoints);

                foreach (var contact in contactPoints)
                {
                    if (!contact.collider) continue;
                    if (contact.collider.gameObject.TryGetComponent(out LedgeBottom ledge))
                    {
                        //Debug.Log("Found A Bottom Ledge, Jumping Up.");
                        int height = Mathf.RoundToInt(Mathf.Abs(ledge.transform.position.y - ledge.ledgetop.transform.position.y));

                        //Debug.Log(height);

                        // Is height too high?
                        if (height >= CommonPlayerState.GetJumpHeight() + 1)
                        {
                            break;
                        }

                        // we do this so players cant jump sideways upwards.
                        manager.directionedObject.direction = Vector2Int.up;
                        manager.animator.SetAnimation(ANIM_JUMP);
                        manager.SwitchState(new JumpingLedgePlayerState(height, ledge.ledgetop.transform.position.y, ledge.ledgetop.layersToDecrease));
                        return;
                    }
                }
            }
            // if either ledge check fails or we just arent near one, do a regular jump.
            manager.animator.SetAnimation(ANIM_JUMP);
            manager.SwitchState(new JumpingPlayerState());
            return;
        }

        //Check Sword
        if (Input.GetButtonDown("Fire1"))
        {
            //find grabbable
            /*ContactPoint2D[] contactPoints = new ContactPoint2D[2];
            manager.rigidBody.GetContacts(contactPoints);
            foreach (var contact in contactPoints)
            {
                if (!contact.collider) continue;
                if (contact.collider.gameObject.TryGetComponent(out Grabbable grabbable))
                {
                    manager.SwitchState(new GrabbingPlayerState(grabbable));
                    return;
                }
            }*/
            bool ColliderThing = CommonPlayerState.ColliderInDirection(manager, out GameObject go);
            if (go)
            {
                if (go.TryGetComponent(out Grabbable grabbable))
                {
                    manager.SwitchState(new GrabbingPlayerState(grabbable));
                    return;
                }
                else if (go.TryGetComponent(out ClimbableWall climbable))
                {
                    manager.SwitchState(new ClimbingPlayerState(climbable.transform.position.y, climbable.transform.position.y + climbable.height));
                    return;
                }
                else if (go.TryGetComponent(out LedgeBottom ledgeBottom))
                {
                    manager.SwitchState(new ClimbingPlayerState(ledgeBottom.transform.position.y, ledgeBottom.ledgetop.transform.position.y));
                    return;
                }
                else if (go.TryGetComponent(out LedgeTop ledgeTop))
                {
                    manager.SwitchState(new ClimbingPlayerState(ledgeTop.bottom.transform.position.y, ledgeTop.transform.position.y)); ;
                    return;
                }
            }

            manager.SwitchState(new SwordPlayerState());
            return;
        }

        /*//Check Shield
        if (Input.GetButtonDown("Shield"))
        {
            manager.SwitchState(new ShieldPlayerState());
        }*/

        // Check if in water
        // we can use a z range checker to only check on the tilemap, say for instance we have a platform above we can stand on.
        ContactFilter2D contactFilter = new()
        {
            layerMask = LayerMask.NameToLayer("Default"),
            useTriggers = true,
            minDepth = manager.transform.position.z,
            maxDepth = manager.transform.position.z + 3
        };
        List<RaycastHit2D> results = new();
        int _ = Physics2D.BoxCast(manager.transform.position + Vector3.down * 0.2f, Vector2.one * 0.01f, 0, Vector2.zero, contactFilter, results, 0);
        foreach (RaycastHit2D hit in results)
        {
            if (hit.collider.TryGetComponent(out Water _))
            {
                manager.SwitchState(new SwimPlayerState());
                return;
            }
        }

        AnimatePlayer(manager);
    }
    void AnimatePlayer(PlayerStateManager manager)
    {
        // Landing
        if (manager.stateTransitionTimer1 > 0 && manager.previousState is JumpingPlayerState or JumpingLedgePlayerState)
        {
            manager.animator.SetAnimation(1);
            return;
        }

        // Moving
        if (manager.rigidBody.velocity != Vector2.zero)
        {
            manager.animator.SetAnimation(2);
            return;
        }

        //Idle
        manager.animator.SetAnimation(0);

    }
}
