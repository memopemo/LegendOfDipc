using System.Collections.Generic;
using UnityEngine;

// Move around, Jump, Sword, and Shield
public class DefaultPlayerState : IPlayerState
{
    const float DEFAULT_SPEED = 6f;

    const int ANIM_IDLE = 0;
    const int ANIM_RUN = 1;
    const int ANIM_STOP_RUN = 1;
    const int ANIM_JUMP = 2;
    const int ANIM_LAND = 3;

    //We specify left to up and up to left because they flip differently for a brief moment.
    const int ANIM_TURN_DIAG_LEFT_TO_UP = 15;
    const int ANIM_TURN_DIAG_UP_TO_LEFT = 16;
    const int ANIM_TURN_DIAG_LEFT_TO_DOWN = 17;
    const int ANIM_TURN_DIAG_DOWN_TO_LEFT = 18;
    const int ANIM_TURN_DIAG_RIGHT_TO_UP = 19;
    const int ANIM_TURN_DIAG_RIGHT_TO_DOWN = 20;

    int runToIdleTransitionFrames;
    const int RUN_TO_IDLE_HOLD_FRAMES = 0;

    int turnFrames;
    int turning; //1 is up, -1 is down, 0 is not.
    const int TURN_HOLD_FRAMES = 5;
    Vector2Int previousFrameDirection;

    void IPlayerState.OnEnter(PlayerStateManager manager)
    {

    }

    void IPlayerState.OnLeave(PlayerStateManager manager)
    {
    }

    void IPlayerState.OnUpdate(PlayerStateManager manager)
    {
        CommonPlayerState.MovePlayerRaw(manager, DEFAULT_SPEED);
        previousFrameDirection = manager.directionedObject.direction;

        CommonPlayerState.UpdateDirection(manager);
        Vector2Int newDirection = manager.directionedObject.direction;

        // check any direction change that has to do with up.
        if (   previousFrameDirection == Vector2Int.left
            && newDirection == Vector2Int.up
            || previousFrameDirection == Vector2Int.up
            && newDirection == Vector2Int.left
            || previousFrameDirection == Vector2Int.right
            && newDirection == Vector2Int.up
            || previousFrameDirection == Vector2Int.up
            && newDirection == Vector2Int.right
            )
        {
            turnFrames = TURN_HOLD_FRAMES;
            turning = 1;
        }
        else
        // check and direction change that has to do with down.
        if (   previousFrameDirection == Vector2Int.left
            && newDirection == Vector2Int.down
            || previousFrameDirection == Vector2Int.down
            && newDirection == Vector2Int.left
            || previousFrameDirection == Vector2Int.right
            && newDirection == Vector2Int.down
            || previousFrameDirection == Vector2Int.down
            && newDirection == Vector2Int.right
            )
        {
            turnFrames = TURN_HOLD_FRAMES;
            turning = -1;
        }

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
                else if(go.TryGetComponent(out Interactable interactable))
                {
                    interactable.Interact();
                    return;
                }
            }

            manager.SwitchState(new SwordPlayerState());
            return;
        }

        //Check Consumable Item Use
        if(Input.GetButtonDown("Fire2"))
        { 
            GameObject[] consumableItems = manager.itemGameObjectLookup.ConsumableItems;
            int[] inventoryConsumableType = SaveManager.GetSave().InventoryConsumableType;
            int selectionIndex = Object.FindAnyObjectByType<InventoryConsumableSelector>(FindObjectsInactive.Include).selectionIndex;
            if(SaveManager.UseUpConsumableItem(selectionIndex))
            {
                GameObject.Instantiate(
                    consumableItems[ //get the gameobject..
                        inventoryConsumableType[ //of the id..
                            selectionIndex]], //..of the slot currently selected in the inventory
                            manager.transform.position + (Vector3)(Vector2)manager.directionedObject.direction,
                            Quaternion.identity
                            );
            }
        }
        if(Input.GetButtonDown("Fire3"))
        {
            GameObject.Instantiate(
                manager.itemGameObjectLookup.UsableKeyItems[ //get the gameobject.. //of the id..
                        GameObject.FindAnyObjectByType<InventoryKeyItemSelector>(FindObjectsInactive.Include).SelectionIndex], //..of the slot currently selected in the inventory
                        manager.transform.position + (Vector3)(Vector2)manager.directionedObject.direction,
                        Quaternion.identity
                        ); 
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
        //find interactable to set flash. This provides a direct indicator that we can interact because it uses the same function to find the interactable object.
        CommonPlayerState.ColliderInDirection(manager, out GameObject g, true);
        if(g && g.TryGetComponent(out Interactable interact))
        {
            interact.PlayerCanInteract();
        }


        AnimatePlayer(manager);
    }
    void AnimatePlayer(PlayerStateManager manager)
    {
        // Landing
        if (manager.stateTransitionTimer1 > 0 && manager.previousState is JumpingPlayerState or JumpingLedgePlayerState)
        {
            manager.animator.SetAnimation(ANIM_LAND);
            return;
        }

        // Moving
        if (manager.rigidBody.velocity != Vector2.zero)
        {
            //Turning (yes its complicated.)
            if (turnFrames > 0)
            {
                if (turning == 1)
                {
                    if (previousFrameDirection == Vector2Int.right)
                    {
                        manager.animator.SetAnimation(ANIM_TURN_DIAG_RIGHT_TO_UP);
                    }
                    //differentiate between left up and up left for mirroring
                    else if (previousFrameDirection == Vector2Int.left
                             && manager.directionedObject.direction == Vector2Int.up)
                    {
                        manager.animator.SetAnimation(ANIM_TURN_DIAG_LEFT_TO_UP);
                    }
                    else if (previousFrameDirection == Vector2Int.up
                             && manager.directionedObject.direction == Vector2Int.left)
                    {
                        manager.animator.SetAnimation(ANIM_TURN_DIAG_UP_TO_LEFT);
                    }
                }
                else
                {
                    if (previousFrameDirection == Vector2Int.right)
                    {
                        manager.animator.SetAnimation(ANIM_TURN_DIAG_RIGHT_TO_DOWN);
                    }
                    //differentiate between left down and down left for mirroring
                    else if (previousFrameDirection == Vector2Int.left
                             && manager.directionedObject.direction == Vector2Int.down)
                    {
                        manager.animator.SetAnimation(ANIM_TURN_DIAG_LEFT_TO_DOWN);
                    }
                    else if (previousFrameDirection == Vector2Int.down
                             && manager.directionedObject.direction == Vector2Int.left)
                    {
                        manager.animator.SetAnimation(ANIM_TURN_DIAG_DOWN_TO_LEFT);
                    }
                }
                turnFrames--;
                return;
            }
            manager.animator.SetAnimation(ANIM_RUN);
            runToIdleTransitionFrames = RUN_TO_IDLE_HOLD_FRAMES;
            return;
        }
        // Transitioning From Moving to Idle
        if (manager.rigidBody.velocity == Vector2.zero && runToIdleTransitionFrames > 0)
        {
            manager.animator.RestartAnimation();
            //Debug.Log(runToIdleTransitionFrames);
            runToIdleTransitionFrames--;
            return;
        }

        //Idle
        manager.animator.SetAnimation(ANIM_IDLE);

    }
}
