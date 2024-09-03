using System;
using System.Collections.Generic;
using UnityEngine;

// Move around, Jump, Sword, and Shield
public class DefaultPlayerState : IPlayerState
{
    const float DEFAULT_SPEED = 6f;

    const int ANIM_IDLE = 0;
    const int ANIM_RUN = 1;
    const int ANIM_STOP_RUN = 1;
    const int ANIM_LAND = 3;
    const int ANIM_WALK = 22;

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
    private const double WALK_THRESHOLD = 0.5;
    Vector2Int previousFrameDirection;
    bool playedStepSound;

    void IPlayerState.OnEnter(PlayerStateManager manager)
    {

    }

    void IPlayerState.OnLeave(PlayerStateManager manager)
    {
    }

    //there will be a HELLA ton of SwitchState then early return statements everywhere.
    // i am too dumb to think of a better way to do this so *shrug* i guess it will look awful. sorry!
    // for extremely sure, i WANT the update function to exit prematurely if we change states.
    void IPlayerState.OnUpdate(PlayerStateManager manager)
    {   
        
        IPlayerState nextState = null; //deffered state transition at the end of the function.
        CommonPlayerState.MovePlayerRaw(manager, DEFAULT_SPEED);

        previousFrameDirection = manager.directionedObject.direction;
        CommonPlayerState.UpdateDirection(manager);
        Vector2Int newDirection = manager.directionedObject.direction;

        // agregious.
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
        if (Buttons.IsButtonDown(Buttons.Jump))
        {
            OnJumpButtonPushed(manager, ref nextState);
        }

        //Check Sword
        if (Buttons.IsButtonDown(Buttons.Sword))
        {
            bool colliderThing = CommonPlayerState.ColliderInDirection(manager, out GameObject go);
            if (colliderThing)
            {
                HandleConfirmButtonNearColliderType(manager, go, ref nextState);
            }
            else //sword is usable.
            {
                nextState = new SwordPlayerState();
            }            
        }

        //Check Consumable Item Use
        if(Buttons.IsButtonDown(Buttons.ConsumableItem))
        {
            CreateConsumableItemObject(manager);
        }
        //Check Key Item Use
        if (Buttons.IsButtonDown(Buttons.KeyItem))
        {
            CreateKeyItemObject(manager);
        }
        if(Buttons.IsButtonDown(Buttons.Shield))
        {
            nextState = new ShieldPlayerState();
        }
        if(Buttons.IsButtonDown(Buttons.Boomerang))
        {
            nextState = new BoomerangPlayerState();
        }

        // Check if in water
        // we can use a z range checker to only check on the tilemap, say for instance we have a platform above we can stand on.
        ContactFilter2D contactFilter = new()
        {
            layerMask = LayerMask.NameToLayer("Default"),
            useTriggers = true,
            minDepth = manager.transform.position.z,
            maxDepth = manager.transform.position.z + 3
            
        };
        Physics2D.queriesHitTriggers = true;
        BoxCastFind<Water>(manager.transform.position + (Vector3.down * 0.2f), Vector2.one * 0.01f, (water) => nextState = new SwimPlayerState(), contactFilter);
        Physics2D.queriesHitTriggers = false;
        //find interactable to set flash. This provides a direct indicator that we can interact because it uses the same function to find the interactable object.
        CommonPlayerState.ColliderInDirection(manager, out GameObject g, true);
        if(g && g.TryGetComponent(out Interactable interact))
        {
            interact.PlayerCanInteract();
        }


        AnimatePlayer(manager);

        if(nextState is not null)
        {
            manager.SwitchState(nextState);
        }
        

        
    }
    public void CheckWallStapleWall(PlayerStateManager manager)
    {
        CommonPlayerState.ColliderInDirection(manager, out GameObject go, true);

        if (go.TryGetComponent(out LedgeBottom ledgeBottom))
        {
            manager.SwitchState(new ClimbingPlayerState(ledgeBottom.transform.position.y, ledgeBottom.ledgetop.transform.position.y));
        }
        else if (go.TryGetComponent(out LedgeTop ledgeTop))
        {
            manager.SwitchState(new ClimbingPlayerState(ledgeTop.bottom.transform.position.y, ledgeTop.transform.position.y));
        }
    }
    void HandleConfirmButtonNearColliderType(PlayerStateManager manager, GameObject go, ref IPlayerState nextState)
    {
        if (go.TryGetComponent(out Grabbable grabbable))
        {
            nextState = new GrabbingPlayerState(grabbable);
        }
        else if (go.TryGetComponent(out ClimbableWall climbable) && manager.directionedObject.direction.x == 0 && SaveManager.GetSave().ObtainedKeyUnselectableItems[3])
        {
            nextState = new ClimbingPlayerState(climbable.transform.position.y, climbable.transform.position.y + climbable.height);
        }
        else if (go.TryGetComponent(out LedgeBottom ledgeBottom) && (GameObject.FindAnyObjectByType<InventoryKeyItemSelector>(FindObjectsInactive.Include).SelectionIndex == 17 && SaveManager.GetSave().ObtainedKeyItems[17]))
        {
            nextState = new ClimbingPlayerState(ledgeBottom.transform.position.y, ledgeBottom.ledgetop.transform.position.y);
        }
        else if (go.TryGetComponent(out LedgeTop ledgeTop) && (GameObject.FindAnyObjectByType<InventoryKeyItemSelector>(FindObjectsInactive.Include).SelectionIndex == 17 && SaveManager.GetSave().ObtainedKeyItems[17]))
        {
            nextState = new ClimbingPlayerState(ledgeTop.bottom.transform.position.y, ledgeTop.transform.position.y);
        }
        else if (go.TryGetComponent(out Interactable interactable))
        {
            interactable.Interact();
        }                 
        else
        {
            nextState = new SwordPlayerState();
        }
    }

    private static void CreateConsumableItemObject(PlayerStateManager manager)
    {
        //Get all information of what we want to instantiate
        GameObject[] consumableItems = manager.itemGameObjectLookup.ConsumableItems;
        int[] inventoryConsumableType = SaveManager.GetSave().InventoryConsumableType;
        int selectionIndex = UnityEngine.Object.FindAnyObjectByType<InventoryConsumableSelector>(FindObjectsInactive.Include).selectionIndex;

        //we want to place it infront of the player
        Vector3 position = manager.transform.position + (Vector3)(Vector2)manager.directionedObject.direction;
        GameObject original = consumableItems[              //get the gameobject..
                                inventoryConsumableType[    //of the id..
                                selectionIndex]];           //..of the slot currently selected in the inventory
        int itemID = inventoryConsumableType[selectionIndex];
        
        if(itemID != 14 || itemID != 15)
        {
            if (!SaveManager.UseUpConsumableItem(selectionIndex)) return; //if we are not clear to create it, then dont. This function also takes care of the inventory item decrement.
        }
        else
        {
            if(!SaveManager.CanUseConsumableItem(selectionIndex)) return; //do not decrement item.
        }
        
        GameObject obj = GameObject.Instantiate(original, position, Quaternion.identity);
        Debug.Log(obj.transform.position);
    }

    private static void CreateKeyItemObject(PlayerStateManager manager)
    {
        //Get all information of what we want to instantiate
        //get inventory's currently selected item index...
        int selectionIndex = GameObject.FindAnyObjectByType<InventoryKeyItemSelector>(FindObjectsInactive.Include).SelectionIndex;
    
        //check if we have the ability to create to create the key item.
        if(!SaveManager.GetSave().ObtainedKeyItems[selectionIndex]) return;

        //get the gameobject of the key item through the lookup table using the index...
        GameObject keyItem = manager.itemGameObjectLookup.UsableKeyItems[selectionIndex];

        //we want to place it infront of the player...
        Vector3 position = manager.transform.position + (Vector3)(Vector2)manager.directionedObject.direction;
        
        //create the object we found.
        GameObject.Instantiate(keyItem, position, Quaternion.identity);
    }

    bool BoxCastFind<T>( Vector2 origin, Vector2 size, Action<T> onFind, ContactFilter2D contactFilter = new())
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

    //If result is true, changing state was successful and should return in main update loop early.
    void OnJumpButtonPushed(PlayerStateManager manager, ref IPlayerState nextState)
    {
        //Find a ledge top if we are jumping downwards.
        if (manager.inputY < 0 && FindTouching(manager.rigidBody, out LedgeTop tledge))
        {
            manager.directionedObject.direction = Vector2Int.down;
            nextState = new JumpingLedgePlayerState(0, tledge.bottom.transform.position.y, tledge.layersToDecrease);
        }

        //Find a ledge side if we are jumping sideways.
        else if (manager.inputX != 0 && FindTouching(manager.rigidBody, out BackSideLedge bsledge) )
        {
            nextState = new JumpingLedgePlayerState(-1, 0, bsledge.layersToDrop);
        }

        //Find a ledge bottom if we are jumping up a ledge.
        else if (manager.inputY > 0 && FindTouching(manager.rigidBody, out LedgeBottom bLedge))
        {
            int height = Mathf.RoundToInt(Mathf.Abs(bLedge.transform.position.y - bLedge.ledgetop.transform.position.y));

            //Debug.Log(height);

            // Is height good?
            if (height < CommonPlayerState.GetJumpHeight() + 1)
            {
                manager.directionedObject.direction = Vector2Int.up;
                nextState = new JumpingLedgePlayerState(height, bLedge.ledgetop.transform.position.y, bLedge.ledgetop.layersToDecrease);
            }
        }
        else
        {
            // if all ledge checks fail or we just arent near one, do a regular jump.
            nextState = new JumpingPlayerState();
        }
    }

    //Return condition: if we found a T.
    bool FindTouching<T>(Rigidbody2D rigidbody2D, out T foundT)
    {
        // look through max of 2 objects we are contacting with.
        ContactPoint2D[] contactPoints = new ContactPoint2D[2];
        rigidbody2D.GetContacts(contactPoints);

        foreach (var contact in contactPoints)
        {
            if (!contact.collider) continue;
            if (contact.collider.gameObject.TryGetComponent(out T thisT))
            {
                foundT = thisT;
                return true;
            }
        }
        foundT = default;
        return false;
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
            if(manager.rawInput.magnitude > WALK_THRESHOLD)
            {
                manager.animator.SetAnimation(ANIM_RUN);
                if(manager.animator.currentAnimationFrame == 0 || manager.animator.currentAnimationFrame == 3)
                {
                    if(!playedStepSound)
                    {
                        manager.noiseMaker.Play(0);
                        playedStepSound = true;
                    }
                }
                else
                {
                    playedStepSound = false;
                }
            }
            else
            {
                manager.animator.SetAnimation(ANIM_WALK);
            }
            
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
