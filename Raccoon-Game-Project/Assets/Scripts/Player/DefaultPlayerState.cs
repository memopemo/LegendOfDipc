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

        //check for direction change for diagonal sprites
        if ((previousFrameDirection + newDirection).y == 1)
        {
            turnFrames = TURN_HOLD_FRAMES;
            turning = 1;
        }
        else if ((previousFrameDirection + newDirection).y == -1)
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
            manager.directionCheck.SetFindTriggers(true);
            //directly find interactables first. We do this so that even if its behind a wall or some bullshit we dont hit what we dont want and dip if its not an interactable.
            if (manager.directionCheck.Evaluate<Interactable>((interact) => { interact.Interact(); })) { }

            //if we do find something, then activate it, else use sword.
            else if (!manager.directionCheck.EvaluateAnything((col) => { HandleConfirmButtonNearColliderType(manager, col.gameObject, ref nextState); }))
            {
                nextState = new SwordPlayerState();
            }
            manager.directionCheck.SetFindTriggers(false);
        }

        //Check Consumable Item Use
        if (Buttons.IsButtonDown(Buttons.ConsumableItem))
        {
            CreateConsumableItemObject(manager);
        }
        //Check Key Item Use
        if (Buttons.IsButtonDown(Buttons.KeyItem))
        {
            CreateKeyItemObject(manager);
        }
        if (Buttons.IsButtonDown(Buttons.Shield))
        {
            nextState = new ShieldPlayerState();
        }
        if (Buttons.IsButtonDown(Buttons.Boomerang))
        {
            nextState = new BoomerangPlayerState();
        }
        manager.waterCheck.Evaluate<Water>((_) => nextState = new SwimPlayerState());
        //find trigger checks that also have interactable.
        manager.directionCheck.SetFindTriggers(true);
        manager.directionCheck.Evaluate<Interactable>((interact) => { interact.PlayerCanInteract(); });
        manager.directionCheck.SetFindTriggers(false);


        AnimatePlayer(manager);

        if (nextState is not null)
        {
            manager.SwitchState(nextState);
        }
    }

    public void CheckWallStapleWall(PlayerStateManager manager)
    {
        Collider2D foundCollider = null;
        if (manager.directionCheck.EvaluateAnything((c) => foundCollider = c))
        {
            if (foundCollider.TryGetComponent(out LedgeBottom ledgeBottom))
            {
                manager.SwitchState(new ClimbingPlayerState(ledgeBottom.transform.position.y, ledgeBottom.ledgetop.transform.position.y));
            }
            else if (foundCollider.TryGetComponent(out LedgeTop ledgeTop))
            {
                manager.SwitchState(new ClimbingPlayerState(ledgeTop.bottom.transform.position.y, ledgeTop.transform.position.y));
            }
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
        else if (go.TryGetComponent(out LedgeBottom ledgeBottom) && (SelectedItem.KeyItem == 17 && SaveManager.GetSave().ObtainedKeyItems[17]))
        {
            nextState = new ClimbingPlayerState(ledgeBottom.transform.position.y, ledgeBottom.ledgetop.transform.position.y);
        }
        else if (go.TryGetComponent(out LedgeTop ledgeTop) && (SelectedItem.KeyItem == 17 && SaveManager.GetSave().ObtainedKeyItems[17]))
        {
            nextState = new ClimbingPlayerState(ledgeTop.bottom.transform.position.y, ledgeTop.transform.position.y);
        }//removed interactable bc now its directly checked.
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
        int selectionIndex = SelectedItem.ConsumableItem;

        //we want to place it infront of the player
        Vector3 position = manager.transform.position + (Vector3)(Vector2)manager.directionedObject.direction;
        GameObject original = consumableItems[              //get the gameobject..
                                inventoryConsumableType[    //of the id..
                                selectionIndex]];           //..of the slot currently selected in the inventory
        int itemID = inventoryConsumableType[selectionIndex];

        if (itemID != 14 || itemID != 15)
        {
            if (!SaveManager.UseUpConsumableItem(selectionIndex)) return; //if we are not clear to create it, then dont. This function also takes care of the inventory item decrement.
        }
        else
        {
            if (!SaveManager.CanUseConsumableItem(selectionIndex)) return; //do not decrement item.
        }

        GameObject obj = GameObject.Instantiate(original, position, Quaternion.identity);
        //Debug.Log(obj.transform.position);
    }

    private static void CreateKeyItemObject(PlayerStateManager manager)
    {
        //Get all information of what we want to instantiate
        //get inventory's currently selected item index...
        int selectionIndex = SelectedItem.KeyItem;

        //check if we have the ability to create to create the key item.
        if (!SaveManager.GetSave().ObtainedKeyItems[selectionIndex]) return;

        //get the gameobject of the key item through the lookup table using the index...
        GameObject keyItem = manager.itemGameObjectLookup.UsableKeyItems[selectionIndex];

        //we want to place it infront of the player...
        Vector3 position = manager.transform.position + (Vector3)(Vector2)manager.directionedObject.direction;

        //create the object we found.
        GameObject.Instantiate(keyItem, position, Quaternion.identity);
    }

    //If result is true, changing state was successful and should return in main update loop early.
    void OnJumpButtonPushed(PlayerStateManager manager, ref IPlayerState nextState)
    {
        CollisionCheck collisionAtBottomCheck = new(manager.GetComponent<Collider2D>());

        //Find a ledge top if we are jumping downwards.
        if (manager.inputY < 0 && CollisionCheck.EvaluateTouching(manager.rigidBody, out LedgeTop tledge))
        {
            //do not jump if we are blocked at the bottom by an object.
            //get bottom y position at our x position.
            collisionAtBottomCheck
            .SetRelativePosition(new Vector2(0, (tledge.bottom.transform.position - manager.transform.position + (Vector3.down * 0.5f)).y))
            .SetBoxSize(0.5f)
            .SetType(CollisionCheck.CollisionType.SingleBox)
            .SetDebug(true);
            if (!collisionAtBottomCheck.EvaluateAnything((_) => { }))
            {
                manager.directionedObject.direction = Vector2Int.down;
                nextState = new JumpingLedgePlayerState(0, tledge.bottom.transform.position.y, tledge.layersToDecrease);
            }
        }

        //Find a ledge side if we are jumping sideways.
        else if (manager.inputX != 0 && CollisionCheck.EvaluateTouching(manager.rigidBody, out BackSideLedge bsledge))
        {
            nextState = new JumpingLedgePlayerState(-1, 0, bsledge.layersToDrop);
        }

        //Find a ledge bottom if we are jumping up a ledge.
        else if (manager.inputY > 0 && CollisionCheck.EvaluateTouching(manager.rigidBody, out LedgeBottom bLedge))
        {
            int height = Mathf.RoundToInt(Mathf.Abs(bLedge.transform.position.y - bLedge.ledgetop.transform.position.y));

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
            if (manager.rawInput.magnitude > WALK_THRESHOLD)
            {
                manager.animator.SetAnimation(ANIM_RUN);
                if (manager.animator.currentAnimationFrame == 0 || manager.animator.currentAnimationFrame == 3)
                {
                    if (!playedStepSound)
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
