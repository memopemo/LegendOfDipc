using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Carry liftable object and throw.
public class LiftPlayerState : IPlayerState
{
    const float CARRY_SPEED = 4.0f;

    Liftable liftable;

    // Custom contructor for referencing the liftable
    public LiftPlayerState(Liftable liftable)
    {
        this.liftable = liftable;
    }

    void IPlayerState.OnEnter(PlayerStateManager manager)
    {
        liftable.transform.SetParent(manager.transform);
        liftable.transform.localPosition = Vector3.zero;
        liftable.colider.enabled = false;
        liftable.heightable.height = 1f;
    }

    void IPlayerState.OnLeave(PlayerStateManager manager)
    {
        liftable.transform.SetParent(null);
        ThrowOrDrop(manager);

    }
    void ThrowOrDrop(PlayerStateManager manager) 
    {
        if (liftable.isThrown)
        {
            //for not colliding with walls.
            liftable.transform.position += Vector3.down * 0.195f;
            liftable.OnThrown(manager.directionedObject.direction);
        }
        else // drop liftable.
        {
            // for colliding with walls and objects.
            liftable.colider.enabled = true;
            liftable.heightable.height = 0;

            //Place infront of player
            liftable.transform.position = manager.transform.position + (Vector3)(Vector2)manager.directionedObject.direction;

            //Snap inside grid (we need to snap it to the nearest *.5*, NOT whole number)
            //TODO: still looks weird.
            liftable.transform.position = Common.SnapToGrid(liftable.transform.position);
        }
    }

    void IPlayerState.OnUpdate(PlayerStateManager manager)
    {
        manager.animator.SetAnimation(14);

        if (manager.stateTransitionTimer1 > 0) 
        {
            //TODO: lifting animation
            return;
        }
        CommonPlayerState.MovePlayerRaw(manager, CARRY_SPEED);
        CommonPlayerState.UpdateDirection(manager);

        if (Input.GetButtonDown("Fire1"))
        {
            // is placable object and do we have space to set it down?
            if (!liftable.isThrown)
            {
                if (CommonPlayerState.ColliderInDirection(manager, out _)) 
                {
                    return;
                }
            }
            //Throw or drop liftable.
            manager.SwitchState(new DefaultPlayerState());
        }
        
    }
}
